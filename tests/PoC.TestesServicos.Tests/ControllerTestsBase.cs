using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PoC.TestesServicos.Data;
using PoC.TestesServicos.Tests.Fixtures;
using Respawn;
using Xunit;

namespace PoC.TestesServicos.Tests
{
    public abstract class ControllerTestsBase : IAsyncLifetime
    {
        private readonly Checkpoint _checkpoint;
        private readonly string _connectionString;

        private readonly IServiceScope _scope;

        public ControllerTestsBase(IntegrationContainersAppFactory integrationContainersFixture)
        {
            Client = integrationContainersFixture.Client;
            _scope = integrationContainersFixture.Server.Host.Services.CreateScope();
            _connectionString = integrationContainersFixture.ConnectionString;
            _checkpoint = new Checkpoint();
            Context = _scope.ServiceProvider.GetRequiredService<UsersDataContext>();
        }

        protected HttpClient Client { get; }
        protected UsersDataContext Context { get; }

        public Task InitializeAsync()
        {
            return _checkpoint.Reset(_connectionString);
        }

        public Task DisposeAsync()
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }
    }
}