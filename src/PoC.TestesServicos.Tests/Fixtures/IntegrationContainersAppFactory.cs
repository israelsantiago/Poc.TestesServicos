using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PoC.TestesServicos.API;
using PoC.TestesServicos.Data;
using WireMock.Server;
using WireMock.Settings;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class IntegrationContainersAppFactory : WebApplicationFactory<Startup>, IAsyncLifetime
    {
        
        public MssqlContainerFixture mssqlContainerFixture { get; }
        public string ConnectionStringDB { get; private set; }
        public CouchbaseContainerFixture couchbaseContainerFixture { get; }
        public TestContextConfiguration TestContextConfigurationDB { get; private set; }
        public HttpClient Client { get; private set; }
        
        public WireMockServer MockServer { get; private set; }        
        private const string CEP_API_URL_SECTION = "CepApiOptions:Url";      
  
   public IntegrationContainersAppFactory()
        {
            mssqlContainerFixture = new MssqlContainerFixture();
            couchbaseContainerFixture = new CouchbaseContainerFixture();
            MockServer = SetupMockedServer();            
           
        }

        public async Task InitializeAsync()
        {
            
            var task1 =  mssqlContainerFixture.InitializeAsync();
            var task2 =  couchbaseContainerFixture.InitializeAsync();            
            
            Task allTasks = Task.WhenAll(task1, task2);
            
            try
            {
                await allTasks;
            }
            catch
            {
                AggregateException allExceptions = allTasks.Exception;
            }            
            
            ConnectionStringDB = mssqlContainerFixture.Container.ConnectionString;
            TestContextConfigurationDB = new TestContextConfiguration(ConnectionStringDB);

            var ConnectionStringCouchBase = couchbaseContainerFixture.Container.ConnectionString;
            
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
            
            Client.Dispose();        
            
            MockServer.Stop();
            MockServer.Dispose();       
            
            return Task.CompletedTask;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Dictionary<string, string> extraConfiguration = GetApiClientExtraConfiguration();
            
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

            }).ConfigureAppConfiguration((context, configbuilder) =>
            {
                configbuilder.AddInMemoryCollection(extraConfiguration);
            }).UseUrls("http://*:0");
         
        }
        
        private WireMockServer SetupMockedServer()
        {
            FluentMockServerSettings settings = new FluentMockServerSettings()
            {
                ReadStaticMappings = true,
                StartAdminInterface = true,
                WatchStaticMappings = true,                
            };

            WireMockServer mockServer = WireMockServer.Start(settings);
            mockServer.ReadStaticMappings("Mappings/");

            return mockServer;
                
        }
        
        /// <summary>
        /// This method creates configurations using mockserver url, that will be used by the API instead of configured in appsettings, this setup allows Wire Mock Server matches the http request mapped.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> GetApiClientExtraConfiguration()
        {
            string requestUrl = MockServer.Urls.Single();
            Dictionary<string, string> configuration = new Dictionary<string, string>();
            configuration.Add(CEP_API_URL_SECTION, requestUrl);
            return configuration;
        }            
        
    }
}