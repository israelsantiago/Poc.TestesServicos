using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Configurations.MessageBrokers;
using DotNet.Testcontainers.Containers.Modules.MessageBrokers;
using RabbitMQ.Client;
using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class RabbitmqContainerFixture
    {
        public RabbitMqTestcontainer Container { get; }
        
        public RabbitmqContainerFixture()
        {
            var testcontainersBuilder = new TestcontainersBuilder<RabbitMqTestcontainer>()
                .WithMessageBroker(new RabbitMqTestcontainerConfiguration
                {
                    Username = "rabbitmq",
                    Password = "rabbitmq",
                });

            Container = testcontainersBuilder.Build();
        }     
        
        public async Task InitializeAsync()
        {
            await Container.StartAsync();
            
            var factory = new ConnectionFactory { Uri = new Uri(Container.ConnectionString) };

            using (var connection = factory.CreateConnection())
            {
                Assert.True(connection.IsOpen);
            }            
        }

        public Task DisposeAsync()
        {
            return Container.DisposeAsync().AsTask();
        }        
    }
}