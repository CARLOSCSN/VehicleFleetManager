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
    public class VehicleRepository: AbstractRepository<Vehicle>
    {
        public VehicleRepository(IConfiguration configuration): base(configuration) { }

        public override void Add(Vehicle item)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO Vehicle (Chassi, Type, NumberPassengers, Color, IsEnabled, CodeFleet)"
                                + " VALUES(@Chassi, @Type, @NumberPassengers, @Color, @IsEnabled, @CodeFleet)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, item);
            }
        }
        public override void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM Vehicle" 
                            + " WHERE Id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public override void Update(Vehicle item)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "UPDATE Vehicle SET Chassi = @Chassi,"
                            + " Type = @Type, NumberPassengers= @NumberPassengers," 
                            + " Color = @Color, CodeFleet= @CodeFleet" 
                            + " WHERE Id = @Id";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Vehicle FindByID(int id)
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM Vehicle" 
                            + " WHERE Id = @Id"
                            + " AND IsEnabled = 1";
                            
                dbConnection.Open();
                var result = dbConnection.Query<Vehicle>(sQuery, new { Id = id }).FirstOrDefault();

                var listVehiclesType = dbConnection.Query<VehicleType>("SELECT * FROM VehicleType WHERE IsEnabled = 1");
                result.VehicleType = listVehiclesType.Where(x => x.Code == result.Type).FirstOrDefault();   

                return result;             
            }
        }
        public override IEnumerable<Vehicle> FindAll()
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                dbConnection.Open();

                var listVehiclesType = dbConnection.Query<VehicleType>("SELECT * FROM VehicleType WHERE IsEnabled = 1");

                string sQuery = "SELECT Vehicle.* FROM Vehicle" 
                            + " INNER JOIN Fleet ON Fleet.Code = Vehicle.CodeFleet"
                            + " AND Vehicle.IsEnabled = 1"
                            + " AND Vehicle.CodeFleet = (SELECT Code FROM Fleet WHERE IsEnabled = 1 AND Current = 1 LIMIT 1)";

                var listVehicles = dbConnection
                    .Query<Vehicle>(sQuery);

                foreach (var vehicle in listVehicles)
                    vehicle.VehicleType = listVehiclesType.Where(x => x.Code == vehicle.Type).FirstOrDefault();

                return listVehicles;
            }
        }

        public IEnumerable<Vehicle> FindAll(string chassiFilter = null)
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                dbConnection.Open();

                var listVehiclesType = dbConnection.Query<VehicleType>("SELECT * FROM VehicleType WHERE IsEnabled = 1");

                string sQuery = "SELECT Vehicle.* FROM Vehicle" 
                            + " INNER JOIN Fleet ON Fleet.Code = Vehicle.CodeFleet"
                            + " AND Vehicle.IsEnabled = 1"
                            + " AND Vehicle.CodeFleet = (SELECT Code FROM Fleet WHERE IsEnabled = 1 AND Current = 1 LIMIT 1)";

                            if(!string.IsNullOrWhiteSpace(chassiFilter)){
                                sQuery = sQuery + " AND Vehicle.Chassi like '%"+ chassiFilter +"%'";
                            }

                var listVehicles = dbConnection
                    .Query<Vehicle>(sQuery);

                foreach (var vehicle in listVehicles)
                    vehicle.VehicleType = listVehiclesType.Where(x => x.Code == vehicle.Type).FirstOrDefault();

                return listVehicles;
            }
        }        
        public Vehicle FindByChassi(string chassi)
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM Vehicle" 
                            + " WHERE Chassi = @Chassi"
                            + " AND IsEnabled = 1";

                dbConnection.Open();

                var result = dbConnection.Query<Vehicle>(sQuery, new { Chassi = chassi }).FirstOrDefault();

                return result;
            }
        }        
    }
}