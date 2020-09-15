using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using DotNet.Testcontainers.Services;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Rabbitmq;
using RabbitMQ.Client;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class RabbitmqContainerFixture: IAsyncLifetime
    {
        public RabbitMqTestcontainer Container { get; }
        
        public int AdminPortRabbitMqHost { get; private set; }

        private const int AdminPortRabbitMq = 15672;

        public RabbitmqContainerFixture()
        {
            AdminPortRabbitMqHost = TestcontainersNetworkService.GetAvailablePort();
            
            var testcontainersBuilder = new TestcontainersBuilder<RabbitMqTestcontainer>()
                .WithMessageBroker(new RabbitMqTestcontainerConfiguration
                {
                    Username = "rabbitmq",
                    Password = "rabbitmq",
                })
                .WithPortBinding(AdminPortRabbitMqHost, AdminPortRabbitMq ) // Rabbitmq admin port;
                 // TODO tratar de forma dinamica a localização do arquivo abaixo...
                .WithMount(  "C:/dev/NET/Poc.TestesServicos/src/PoC.TestesServicos.Test/Fixtures/Configurations/Messagebrokers/Rabbitmq/definitions.json",
                          "/etc/rabbitmq/definitions.json");  // Convenção sobre configuração, nesta versão do RabbitMQ 3.8.5, se o arquivo definitions.json for 'montado'
                                                                       // em /etc/rabbitmq/definitions, no startup do RabbitMQ, as definições serão importandas.
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