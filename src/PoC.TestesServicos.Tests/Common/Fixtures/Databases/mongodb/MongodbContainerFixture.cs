using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules.Databases;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Databases;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class MongodbContainerFixture : IAsyncLifetime
    {
        public MongoDbTestcontainer Container { get; }        
        
        public MongodbContainerFixture()
        {
            var testcontainersBuilder = new TestcontainersBuilder<MongoDbTestcontainer>()
                    //TODO .WithImage() -- Alterar para a imagem do BS2
                    .WithDatabase(new MongodbTestcontainerConfiguration()
                    {
                        Username = "mongodbuser",
                        Password = "password",
                        Database = "mogodbtest"
                    })
                    .WithMount(   "./Common/Fixtures/Databases/mongodb/mongo-init.js",
                               "/docker-entrypoint-initdb.d/mongo-init.js"); // https://stackoverflow.com/questions/42912755/how-to-create-a-db-for-mongodb-container-on-start-up
                                                                                      // https://docs.mongodb.com/manual/reference/method/
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