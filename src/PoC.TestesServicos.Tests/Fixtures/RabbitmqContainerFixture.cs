using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using PoC.TestesServicos.Tests.Fixtures.Configurations.Rabbitmq;
using RabbitMQ.Client;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class RabbitmqContainerFixture: IAsyncLifetime
    {
        public RabbitMqTestcontainer Container { get; }

        public RabbitmqContainerFixture()
        {
            var testcontainersBuilder = new TestcontainersBuilder<RabbitMqTestcontainer>()
                .WithMessageBroker(new RabbitMqTestcontainerConfiguration
                {
                    Username = "rabbitmq",
                    Password = "rabbitmq",
                })
                .WithPortBinding(15672) // Rabbitmq admin port;
                .WithMount(  "D:/dev/NET/Poc.TestesServicos/src/PoC.TestesServicos.Tests/rabbitmq/etc/definitions.json",
                          "/etc/rabbitmq/definitions.json");  // Convenção sobre configuração, nesta versão do RabbitMQ 3.8.5, se o arquivo acima for 'montado'
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