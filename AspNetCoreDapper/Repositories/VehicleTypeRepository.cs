using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using AspNetCoreDapper.Models;

namespace AspNetCoreDapper.Repositories
{
    public class VehicleTypeRepository: AbstractRepository<VehicleType>
    {
        public VehicleTypeRepository(IConfiguration configuration): base(configuration) { }

        public override void Add(VehicleType item)
        {
            throw new NotImplementedException();
        }

        public override VehicleType FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public override void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(VehicleType item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<VehicleType> FindAll()
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                dbConnection.Open();

                return dbConnection.Query<VehicleType>("SELECT * FROM VehicleType WHERE IsEnabled = 1");
            }
        }        
    }
}