using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules.Databases;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Databases;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class CouchbaseContainerFixture : IAsyncLifetime
    {
        private const string UserNameCouchBase = "Administrator";
        private const string PassWordCouchBase = "password";
        public  string BucketName { get; private set; }= "default";   
      
        public CouchbaseTestcontainer Container { get; }
        
        public CouchbaseContainerFixture()
        {

            var testcontainersBuilder = new TestcontainersBuilder<CouchbaseTestcontainer>()
                .WithDatabase(new CouchbaseTestcontainerConfiguration
                {
                    Username = UserNameCouchBase,
                    Password = PassWordCouchBase,
                    BucketName = BucketName
                })
                .WithPortBinding(8091)
                .WithPortBinding(8093)
                .WithPortBinding(11210);
     
            Container = testcontainersBuilder.Build();
      
        }

        public Task InitializeAsync()
        {
            return Container.StartAsync();

        }
       
        public Task DisposeAsync()
        {
            return Container.DisposeAsync().AsTask();
        }
        
    }
}