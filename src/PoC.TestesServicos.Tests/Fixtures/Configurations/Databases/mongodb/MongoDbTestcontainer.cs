namespace PoC.TestesServicos.Tests.Fixtures.Configurations.Databases
{
    using DotNet.Testcontainers.Containers.Configurations;
    using DotNet.Testcontainers.Containers.Modules.Abstractions;
    
    public sealed class MongoDbTestcontainer : TestcontainerDatabase
    {
        internal MongoDbTestcontainer(ITestcontainersConfiguration configuration) : base(configuration)
        {
        }

        public override string ConnectionString => $"mongodb://{this.Hostname}:{this.Port}/{this.Database}";
  
    }    
}