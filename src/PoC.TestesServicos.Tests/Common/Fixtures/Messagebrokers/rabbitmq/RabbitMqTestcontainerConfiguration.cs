using DotNet.Testcontainers.Containers.Configurations.Abstractions;
using DotNet.Testcontainers.Containers.WaitStrategies;

namespace PoC.TestesServicos.Tests.Fixtures.Configurations.Rabbitmq
{
    public class RabbitMqTestcontainerConfiguration: TestcontainerMessageBrokerConfiguration
    {
        // TODO .WithImage() -- Alterar para a imagem do BS2
        private const string RabbitMqImage = "rabbitmq:3.8.5-management";
        private const int RabbitMqPort = 5672;

        public RabbitMqTestcontainerConfiguration()
            : this(RabbitMqImage)
        {
        }

        public RabbitMqTestcontainerConfiguration(string image)
            : base(image, RabbitMqPort)
        {
        }

        public override string Username
        {
            get => this.Environments["RABBITMQ_DEFAULT_USER"];
            set => this.Environments["RABBITMQ_DEFAULT_USER"] = value;
        }

        public override string Password
        {
            get => this.Environments["RABBITMQ_DEFAULT_PASS"];
            set => this.Environments["RABBITMQ_DEFAULT_PASS"] = value;
        }

        public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer().UntilPortIsAvailable(this.DefaultPort);
        
    }        
 
}