using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PoC.TestesServicos.API;
using PoC.TestesServicos.Data;
using PoC.TestesServicos.Tests.Fixtures;
using Respawn;
using Xunit;

namespace PoC.TestesServicos.Tests.Service.Common
{
    public abstract class ControllerTestsBase : IAsyncLifetime
    {
        private readonly Checkpoint _checkpoint;
        private readonly string _connectionStringSqlServer;
        private readonly IServiceScope _scope;
        protected HttpClient Client { get; }
        protected UsersDataContext Context { get; }

        public ControllerTestsBase(IntegrationTestFixture<StartupApiTests> integrationContainersFixture)
        {
            Client = integrationContainersFixture.Client;
            _scope = integrationContainersFixture.Factory.Server.Host.Services.CreateScope();
            _connectionStringSqlServer = integrationContainersFixture.ConnectionStringSqlServer;
            _checkpoint = new Checkpoint();
            Context = _scope.ServiceProvider.GetRequiredService<UsersDataContext>();
        }

   

        public Task InitializeAsync()
        {
            return _checkpoint.Reset(_connectionStringSqlServer);
        }

        public Task DisposeAsync()
        {
            _scope.Dispose();
            return Task.CompletedTask;
        }
    }
}