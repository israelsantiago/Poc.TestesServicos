using System;
using PoC.TestesServicos.Data;

namespace PoC.TestesServicos.API.Configs
{
    public class DataContextConfiguration : IContextConfiguration
    {
        public DataContextConfiguration()
        {
            ConnectionString = Environment.GetEnvironmentVariable("SQL_SERVER_CONNECTION_STRING");
        }

        public string ConnectionString { get; }
    }
}