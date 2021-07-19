using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace AspNetCoreDapperTests
{
    public class Base
    {
        public IConfiguration _config;
        public Base()
        {

            string pathDb = Path.Combine(Directory
                .GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "AspNetCoreDapper\\TestDb.sqlite");

            var myConfiguration = new Dictionary<string, string>
            {
                {"DBInfo:DbFilePath", pathDb.ToString()},
                {"DBInfo:ConnectionString", "Data Source=" + pathDb.ToString()}
            };

            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
        }
    }
}
