using PoC.TestesServicos.Data;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class TestConfigurationDb : IContextConfiguration
    {
        public TestConfigurationDb(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}