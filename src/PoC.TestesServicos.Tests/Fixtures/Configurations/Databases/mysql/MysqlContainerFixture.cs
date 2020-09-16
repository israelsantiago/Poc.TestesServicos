using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Databases.mysql;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures.Configurations.Databases.mysql
{
    public class MysqlContainerFixture : IAsyncLifetime
    {

        public MySqlTestcontainer Container { get; }        
        
        public MysqlContainerFixture()
        {
            var testcontainersBuilder = new TestcontainersBuilder<MySqlTestcontainer>()
                    //TODO .WithImage() -- Alterar para a imagem do BS2
                    .WithDatabase(new MySqlTestcontainerConfiguration
                    {
                        Database = "db",
                        Username = "mysql",
                        Password = "mysql"
                    })
                ;

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