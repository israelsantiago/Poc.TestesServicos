using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PoC.TestesServicos.Data;
using PoC.TestesServicos.Tests.Fixtures;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Databases.mysql;
using WireMock.Server;
using WireMock.Settings;
using Xunit;

namespace PoC.TestesServicos.Tests.Service.Common
{
    [CollectionDefinition(nameof(IntegrationApiTestFixtureCollection))]
    public class IntegrationApiTestFixtureCollection : ICollectionFixture<IntegrationTestFixture<StartupApiTests>>
    {
    }        
    public class IntegrationTestFixture<TStartup> : IDisposable,  IAsyncLifetime where TStartup : class
    {
        public MssqlContainerFixture MssqlContainerFixture { get; }
        public string ConnectionStringSqlServer { get; private set; }
        
        public CouchbaseContainerFixture CouchbaseContainerFixture { get; }
        public string HostCouchbase { get; private set; }
        public string UserNameCouchBase { get; private set; }        
        public string PasswordCouchbase { get; private set; }
        public string BucketName { get; private set; }
        private RabbitmqContainerFixture RabbitmqContainerFixture { get; }     
        public MongodbContainerFixture MongodbContainerFixture { get; }       
        public MysqlContainerFixture MysqlContainerFixture { get; }               
        public WireMockServer MockServer { get; private set; }    
        public string MockeServerUrl { get; private set; }       

        public IntegrationContainersAppFactory<TStartup> Factory;
        public HttpClient Client { get; private set; }

        public  IntegrationTestFixture()
        {
            MssqlContainerFixture = new MssqlContainerFixture();
            CouchbaseContainerFixture = new CouchbaseContainerFixture();
            RabbitmqContainerFixture = new RabbitmqContainerFixture();
            MongodbContainerFixture = new MongodbContainerFixture();
            MysqlContainerFixture = new MysqlContainerFixture();
            MockServer = SetupMockedServer();
        }
      
        public async Task InitializeAsync()
        {
  
            var task1 = MssqlContainerFixture.InitializeAsync();
            var task2 = CouchbaseContainerFixture.InitializeAsync();
            var task3 = RabbitmqContainerFixture.InitializeAsync();
            var task4 = MongodbContainerFixture.InitializeAsync();
            var task5 = MysqlContainerFixture.InitializeAsync();

            Task allTasks = Task.WhenAll(task1, task2, task3, task4, task5); 
            
            try
            {
                await allTasks;
            }
            catch
            {
                AggregateException allExceptions = allTasks.Exception;
            }            
            
            ConnectionStringSqlServer = MssqlContainerFixture.Container.ConnectionString;

            HostCouchbase = CouchbaseContainerFixture.Container.ConnectionString;
            UserNameCouchBase = CouchbaseContainerFixture.Container.Username;
            PasswordCouchbase = CouchbaseContainerFixture.Container.Password;
            BucketName = CouchbaseContainerFixture.BucketName;
 
            MockeServerUrl = MockServer.Urls.Single();
            
            Factory = new IntegrationContainersAppFactory<TStartup>(ConnectionStringSqlServer, HostCouchbase, UserNameCouchBase, PasswordCouchbase, BucketName,  MockeServerUrl);

            var clientOptions = new WebApplicationFactoryClientOptions()
            {
                HandleCookies = false,
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 7
            };
            
            Client = Factory.CreateClient(clientOptions);
            
            using (var scope = Factory.Server.Host.Services.CreateScope())
            {
              var scopedServices = scope.ServiceProvider;
              var context = scopedServices.GetRequiredService<UsersDataContext>();

              context.Database.Migrate();
            }
        }
      
        private WireMockServer SetupMockedServer()
        {
            WireMockServerSettings settings = new WireMockServerSettings()
            {
                ReadStaticMappings = true,
                StartAdminInterface = true,
                WatchStaticMappings = true,                
            };

            WireMockServer mockServer = WireMockServer.Start(settings);
            mockServer.ReadStaticMappings("Common/Fixtures/Servicevirtualizations/wiremock.net/mappings/");

            return mockServer;
        }

        public Task DisposeAsync()
        {
            this?.Dispose();
            return Task.CompletedTask;
        }
        
        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
            
            MssqlContainerFixture.DisposeAsync();
            CouchbaseContainerFixture.DisposeAsync();
            RabbitmqContainerFixture.DisposeAsync();
            MongodbContainerFixture.DisposeAsync();
            MysqlContainerFixture.DisposeAsync();
            
            MockServer.Stop();
            MockServer.Dispose();  
            
        }        
    }
}