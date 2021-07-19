using System;
using Dapper;
using System.IO;
using System.Data;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreDapper.Db
{
    public class Seed
    {
        private static IDbConnection _dbConnection;

        public static void CreateDb(IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("DBInfo:ConnectionString");
            var dbFilePath = configuration.GetValue<string>("DBInfo:DbFilePath");
            if (File.Exists(dbFilePath))
            {
                return;
            }

            //SqliteConnection.CreateFile(dbFilePath);
            _dbConnection = new SqliteConnection(connectionString);
            _dbConnection.Open();

            // Create a Vehicle Type table
            _dbConnection.Execute(@"

                    CREATE TABLE IF NOT EXISTS [Fleet] (
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [Code] NVARCHAR(128) UNIQUE NOT NULL,
                        [Name] NVARCHAR(128) NOT NULL,
                        [IsEnabled] BIT NOT NULL,
                        [Current] BIT NOT NULL
                    );

                    INSERT INTO Fleet (Code, Name, IsEnabled, Current) VALUES('frota01', 'Frota 01', 1, 1);

                    CREATE TABLE IF NOT EXISTS [VehicleType] (
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [Code] NVARCHAR(128) UNIQUE NOT NULL,
                        [Name] NVARCHAR(128) NOT NULL,
                        [IsEnabled] BIT NOT NULL
                    );
                    
                    INSERT INTO VehicleType (Code, Name, IsEnabled) VALUES('bus', 'Ônibus', 1);
                    INSERT INTO VehicleType (Code, Name, IsEnabled) VALUES('truck', 'Caminhão', 1);

                    CREATE TABLE IF NOT EXISTS [Vehicle] (
                        [Id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                        [Chassi] NVARCHAR(128) NOT NULL,
                        [Type] NVARCHAR(128) NOT NULL,
                        [NumberPassengers] INT NOT NULL,
                        [Color] NVARCHAR(64) NULL,
                        [IsEnabled] BIT NOT NULL,
                        [CodeFleet] NVARCHAR(128) NOT NULL,
                        CONSTRAINT fk_Vehicle_VehicleType foreign key (Type) references VehicleType (Code),
                        CONSTRAINT fk_Vehicle_Fleet foreign key (CodeFleet) references Fleet (Code)
                    )                   
                    
                    ");

            _dbConnection.Close();

        }
    }
}