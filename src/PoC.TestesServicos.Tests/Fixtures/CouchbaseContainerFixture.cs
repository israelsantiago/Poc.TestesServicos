using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using DotNet.Testcontainers.Services;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class CouchbaseContainerFixture : IAsyncLifetime
    {
        private const string UserNameCouchBase = "couchbase";
        private const string PassWordCouchBase = "couchbase";
        private const string BucketName = "customers";   
      
        public CouchbaseTestcontainer Container { get; }
        
        public CouchbaseContainerFixture()
        {

            var testcontainersBuilder = new TestcontainersBuilder<CouchbaseTestcontainer>()
                .WithDatabase(new CouchbaseTestcontainerConfiguration
                {
                    Username = "couchbase",
                    Password = "couchbase",
                    BucketName = "customers"
                })
                .WithPortBinding(8091)
                .WithPortBinding(8093)
                .WithPortBinding(11210);
     
            Container = testcontainersBuilder.Build();
      
        }

        public async Task InitializeAsync()
        {
            await Container.StartAsync();
        }
       
        public Task DisposeAsync()
        {
            return Container.DisposeAsync().AsTask();
        }
        
    }
}