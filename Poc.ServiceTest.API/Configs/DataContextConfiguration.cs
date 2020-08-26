using Microsoft.Extensions.Configuration;
using PoC.TestesServicos.Data;

namespace PoC.TestesServicos.API.Configs
{
    public class DataContextConfiguration : IContextConfiguration
    {
        public DataContextConfiguration(IConfiguration config)
        {
            ConnectionString = config.GetConnectionString("UsersDb");
        }

        public string ConnectionString { get; }
    }
}