using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules.Databases;
using DotNet.Testcontainers.Services;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Databases;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class CouchbaseContainerFixture : IAsyncLifetime
    {
        private const string UserNameCouchBase = "couchbase";
        private const string PassWordCouchBase = "couchbase";
        private const string BucketName = "customers";   
        
        public int restPortHost { get; private set; }
        public int queryPortHost { get; private set; }
 
        public CouchbaseTestcontainer Container { get; }
        
        public CouchbaseContainerFixture()
        {
            restPortHost = TestcontainersNetworkService.GetAvailablePort();
            queryPortHost = TestcontainersNetworkService.GetAvailablePort();
     
            var testcontainersBuilder = new TestcontainersBuilder<CouchbaseTestcontainer>()
                //.WithImage()  -- Alterar para a imagem do BS2
                .WithDatabase(new CouchbaseTestcontainerConfiguration(restPortHost, restPortHost)
                {
                    Username = UserNameCouchBase,
                    Password = PassWordCouchBase,
                    BucketName = BucketName,
                })
                .WithExposedPort(8091)
                .WithExposedPort(8093)
                .WithPortBinding(restPortHost, 8091)
                .WithPortBinding(queryPortHost, 8093)
                .WithEnvironment("REST_PORT_HOST", Convert.ToString(restPortHost))
                .WithEnvironment("QUERY_PORT_HOST", Convert.ToString(queryPortHost))
                .WithCommand("/bin/sh", 
                            $" /opt/couchbase/bin/couchbase-cli setting-alternate-address -c 127.0.0.1:8091 --username {UserNameCouchBase} --password {PassWordCouchBase} --set --node 127.0.0.1 --hostname 127.0.0.1",  
                            $" --ports mgmt={restPortHost},n1ql={queryPortHost}");
            
            Container = testcontainersBuilder.Build();
            
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
      
        }

        
        public Task DisposeAsync()
        {
            return Container.DisposeAsync().AsTask();
            
            /*
            if (Host == null) return;
            try
            {
                await Host.StopAsync();
                Host.Dispose();
            }
            catch (Exception ex)
            {
                _messageSink.OnMessage(new DiagnosticMessage("Error occured while cleaning up resources: {0}", ex));
                throw new Exception(result.FinalException.Message);
            } 
            */           
            
        }
        
    }
}