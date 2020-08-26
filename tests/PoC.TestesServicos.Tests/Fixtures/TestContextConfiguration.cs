using PoC.TestesServicos.Data;

namespace PoC.TestesServicos.Tests.Fixtures
{
    public class TestContextConfiguration : IContextConfiguration
    {
        public TestContextConfiguration(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }
    }
}