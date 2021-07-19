using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AspNetCoreDapperTests
{
    public class Base
    {
        public IConfiguration _config;
        public Base()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"DBInfo:DbFilePath", "./TestDb.sqlite"},
                {"DBInfo:ConnectionString", "Data Source=./TestDb.sqlite;"}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }
    }
}
