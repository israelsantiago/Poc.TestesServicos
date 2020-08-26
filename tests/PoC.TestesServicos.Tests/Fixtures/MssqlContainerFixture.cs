using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.Databases;
using DotNet.Testcontainers.Containers.Modules.Databases;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class MssqlContainerFixture : IAsyncLifetime
    {
        public MssqlContainerFixture()
        {
            var testcontainersBuilder = new TestcontainersBuilder<MsSqlTestcontainer>()
                    .WithDatabase(new MsSqlTestcontainerConfiguration
                    {
                        Password =
                            "yourStrong(!)Password" // See following password policy: https://hub.docker.com/r/microsoft/mssql-server-linux/
                    })
                ;

            Container = testcontainersBuilder.Build();
        }

        public MsSqlTestcontainer Container { get; }

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