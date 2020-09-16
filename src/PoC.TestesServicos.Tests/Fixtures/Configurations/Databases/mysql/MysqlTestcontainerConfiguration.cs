namespace PoC.TestesServicos.Tests.Fixtures.Configurations.Databases.mysql
{
    
    using DotNet.Testcontainers.Containers.Configurations.Abstractions;
    using DotNet.Testcontainers.Containers.WaitStrategies;
    public class MySqlTestcontainerConfiguration : TestcontainerDatabaseConfiguration
    {
        private const string MySqlImage = "mysql:5.7"; // TODO trocar para imagens BS2

        private const int MySqlPort = 3306;

        public MySqlTestcontainerConfiguration()
            : this(MySqlImage)
        {
        }

        public MySqlTestcontainerConfiguration(string image)
            : base(image, MySqlPort)
        {
            this.Environments["MYSQL_ALLOW_EMPTY_PASSWORD"] = "yes";
        }

        public override string Database
        {
            get => this.Environments["MYSQL_DATABASE"];
            set => this.Environments["MYSQL_DATABASE"] = value;
        }

        public override string Username
        {
            get => this.Environments["MYSQL_USER"];
            set => this.Environments["MYSQL_USER"] = value;
        }

        public override string Password
        {
            get => this.Environments["MYSQL_PASSWORD"];
            set => this.Environments["MYSQL_PASSWORD"] = value;
        }

        public override IWaitForContainerOS WaitStrategy => Wait.ForUnixContainer()
            .UntilCommandIsCompleted($"mysql --host='localhost' --port='{this.DefaultPort}' --user='{this.Username}' --password='{this.Password}' --protocol=TCP --execute 'SHOW DATABASES;'");
    }  
}