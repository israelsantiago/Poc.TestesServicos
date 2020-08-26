using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PoC.TestesServicos.API;
using PoC.TestesServicos.Data;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class IntegrationContainersAppFactory : WebApplicationFactory<Startup>, IAsyncLifetime
    {
        public IntegrationContainersAppFactory()
        {
            mssqlContainerFixture = new MssqlContainerFixture();
            couchbaseContainerFixture = new CouchbaseContainerFixture();
        }

        public MssqlContainerFixture mssqlContainerFixture { get; }
        public CouchbaseContainerFixture couchbaseContainerFixture { get; }
        public string ConnectionString { get; private set; }
        public TestContextConfiguration TestContextConfigurationDB { get; private set; }
        public HttpClient Client { get; private set; }

        public async Task InitializeAsync()
        {
            await mssqlContainerFixture.InitializeAsync();
            ConnectionString = mssqlContainerFixture.Container.ConnectionString;
            TestContextConfigurationDB = new TestContextConfiguration(ConnectionString);

            await couchbaseContainerFixture.InitializeAsync();
            var ConnectionStringCouchBase = couchbaseContainerFixture.Container.ConnectionString;

            /*
            using (var cluster = await Cluster.ConnectAsync("couchbase://localhost", "couchbase", "couchbase"))
            {
                var buckets = await cluster.Buckets.GetAllBucketsAsync();

                await using (var bucket = await cluster.BucketAsync("customers"))
                {
                    var collection = bucket.DefaultCollection();

                    await cluster.QueryAsync<long>($"CREATE PRIMARY INDEX `#primary` ON `Customers`");

                    await collection.InsertAsync("1", new { Name = ".NET Testcontainers" });

                }
            }
            */

            Client = CreateClient();

            using (var scope = Server.Host.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices
                    .GetRequiredService<UsersDataContext>();

                context.Database.Migrate();
            }
        }

        public Task DisposeAsync()
        {
            mssqlContainerFixture.DisposeAsync();
            couchbaseContainerFixture.DisposeAsync();
            return Task.CompletedTask;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();
                services.Replace(new ServiceDescriptor(typeof(IContextConfiguration), TestContextConfigurationDB));

                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                configuration["Couchbase:Hosts"] = "localhost";
                configuration["Couchbase:Username"] = couchbaseContainerFixture.Container.Username;
                configuration["Couchbase:Password"] = couchbaseContainerFixture.Container.Password;
                configuration["Couchbase:UIPort"] = "8091";

                services.Replace(new ServiceDescriptor(typeof(IConfiguration), configuration));

                /*
                var srv = services.FirstOrDefault(p => p.ServiceType == typeof(ICouchbaseProvider));
                var ret = services.Remove(srv);

                srv = services.FirstOrDefault(p => p.ServiceType == typeof(IDocumentsRepository));
                ret = services.Remove(srv);

                services.AddSingleton<ICouchbaseProvider, CouchbaseProvider>();
                services.AddSingleton<IDocumentsRepository, DocumentsRepository>();
                */
            });
        }
    }
}