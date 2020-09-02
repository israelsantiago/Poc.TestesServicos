﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Testcontainers.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PoC.TestesServicos.API;
using PoC.TestesServicos.Data;
using WireMock.Server;
using WireMock.Settings;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    [CollectionDefinition(nameof(IntegrationApiTestFixtureCollection))]
    public class IntegrationApiTestFixtureCollection : ICollectionFixture<IntegrationTestFixture<StartupApiTests>>
    {
    }        
   
    public class IntegrationTestFixture<TStartup> : IDisposable,  IAsyncLifetime where TStartup : class
    {
        
        public MssqlContainerFixture mssqlContainerFixture { get; }
        public string ConnectionStringDB { get; private set; }
        
        public CouchbaseContainerFixture couchbaseContainerFixture { get; }
        public TestContextConfiguration TestContextConfigurationDB { get; private set; }
        private RabbitmqContainerFixture rabbitmqContainerFixture { get; }        
        
        public WireMockServer MockServer { get; private set; }        
   
        
        public IntegrationContainersAppFactory<TStartup> Factory;
        public HttpClient Client { get; private set; }

        public  IntegrationTestFixture()
        {
            
            mssqlContainerFixture = new MssqlContainerFixture();
            couchbaseContainerFixture = new CouchbaseContainerFixture();
            rabbitmqContainerFixture = new RabbitmqContainerFixture();
            MockServer = SetupMockedServer();
            
        }
        
        public async Task InitializeAsync()
        {
            
            var task1 = mssqlContainerFixture.InitializeAsync();
            var task2 = couchbaseContainerFixture.InitializeAsync();
            var task3 = rabbitmqContainerFixture.InitializeAsync();
            
            Task allTasks = Task.WhenAll(task1, task2, task3);
            
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

            var clientOptions = new WebApplicationFactoryClientOptions()
            {
                HandleCookies = false,
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 7
            };

            var ConnectionStringCouchBase = couchbaseContainerFixture.Container.ConnectionString;
            var UserNameCouchBase = couchbaseContainerFixture.Container.Username;
            var PasswordCouchbase = couchbaseContainerFixture.Container.Password;
            var RestPortCouchbase = couchbaseContainerFixture.restPortHost;
            
            var  MockeServerUrl = MockServer.Urls.Single();
            
            Factory = new IntegrationContainersAppFactory<TStartup>(TestContextConfigurationDB, UserNameCouchBase, 
                                                                    PasswordCouchbase, RestPortCouchbase, 
                                                                    MockeServerUrl);
            Client = Factory.CreateClient(clientOptions);
            
            
            using (var scope = Factory.Server.Host.Services.CreateScope())
            {
              var scopedServices = scope.ServiceProvider;
              var context = scopedServices.GetRequiredService<UsersDataContext>();

              context.Database.Migrate();
            }
                        
        }

        public Task DisposeAsync()
        {
            this.Dispose();
            return Task.CompletedTask;
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
        
        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
            
            mssqlContainerFixture.DisposeAsync();
            couchbaseContainerFixture.DisposeAsync();
            rabbitmqContainerFixture.DisposeAsync();            
            
            MockServer.Stop();
            MockServer.Dispose();  
            
        }        
        
    }
}