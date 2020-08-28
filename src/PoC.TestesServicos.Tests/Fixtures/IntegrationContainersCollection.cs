using Xunit;

namespace PoC.TestesServicos.Tests.Fixtures
{
    [CollectionDefinition("Integration containers collection")]
    public class IntegrationContainersCollection : ICollectionFixture<IntegrationContainersAppFactory>
    {
    }
}