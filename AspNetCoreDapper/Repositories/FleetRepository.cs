using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using Microsoft.Data.Sqlite;
using AspNetCoreDapper.Models;
using System.Threading.Tasks;

namespace AspNetCoreDapper.Repositories
{
    public class FleetRepository: AbstractRepository<Fleet>
    {
        public FleetRepository(IConfiguration configuration): base(configuration) { }

        public override void Add(Fleet item)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "INSERT INTO Fleet (Code, Name, IsEnabled, Current)"
                                + " VALUES(@Code, @Name, @IsEnabled, @Current)";
                dbConnection.Open();
                dbConnection.Execute(sQuery, item);
            }
        }
        public override void Remove(int id)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "DELETE FROM Fleet" 
                            + " WHERE Id = @Id";
                dbConnection.Open();
                dbConnection.Execute(sQuery, new { Id = id });
            }
        }
        public override void Update(Fleet item)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "UPDATE Fleet SET Name = @Name,"
                            + " Code = @Code" 
                            + " WHERE Code = @Code";
                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        }
        public override Fleet FindByID(int id)
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM Fleet" 
                            + " WHERE Id = @Id"
                            + " AND IsEnabled = 1";
                            
                dbConnection.Open();
                return dbConnection.Query<Fleet>(sQuery, new { Id = id }).FirstOrDefault();
            }
        }
        public override IEnumerable<Fleet> FindAll()
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                dbConnection.Open();

                return dbConnection.Query<Fleet>("SELECT * FROM Fleet WHERE IsEnabled = 1");
            }
        }    

        public void Current(string code)
        {
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                var item = new Fleet{ Code = code };               
                string sQuery = "UPDATE Fleet SET Current = 0;";
                sQuery = sQuery + " UPDATE Fleet SET Current = 1 WHERE Code = @Code";

                dbConnection.Open();
                dbConnection.Query(sQuery, item);
            }
        } 

        public string FindCurrentFleet()
        { 
            using (IDbConnection dbConnection = new SqliteConnection(ConnectionString))
            {
                string sQuery = "SELECT * FROM Fleet" 
                            + " WHERE IsEnabled = 1"
                            + " AND Current = 1";
                            
                dbConnection.Open();
                var result = dbConnection.Query<Fleet>(sQuery).FirstOrDefault();
                return result?.Code;
            }
        }                          
    }
}