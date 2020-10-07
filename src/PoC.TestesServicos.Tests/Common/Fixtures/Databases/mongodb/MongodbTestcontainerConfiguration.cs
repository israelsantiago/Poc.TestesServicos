namespace PoC.TestesServicos.Tests.Fixtures.Configurations.Databases
{
    using DotNet.Testcontainers.Containers.Configurations.Abstractions;
    using DotNet.Testcontainers.Containers.WaitStrategies;
    
    public class MongodbTestcontainerConfiguration : TestcontainerDatabaseConfiguration
    {
        private const string MongodbImage = "mongo:4.4.0"; // TODO Alterar para a imagem BS2!

        private const int MongodbPort = 27017;

        public MongodbTestcontainerConfiguration()
            : this(MongodbImage)
        {
        }

        public MongodbTestcontainerConfiguration(string image)
            : base(image, MongodbPort)
        {
            this.Environments["MONGO_INITDB_DATABASE"] = "mongodbtest";
        }

        public override string Database
        {
            get => this.Environments["MONGO_INITDB_DATABASE"];
            set => this.Environments["MONGO_INITDB_DATABASE"] = value;        }

        public override string Username
        {
            get => this.Environments["MONGO_INITDB_ROOT_USERNAME"];
            set => this.Environments["MONGO_INITDB_ROOT_USERNAME"] = value;
        }

        public override string Password
        {
            get => this.Environments["MONGO_INITDB_ROOT_PASSWORD"];
            set => this.Environments["MONGO_INITDB_ROOT_PASSWORD"] = value;
        }

        public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer()
            .UntilPortIsAvailable(MongodbPort);
    }    
}