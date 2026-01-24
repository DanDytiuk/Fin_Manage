using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace FinManage.Services
{
    public class DataBaseWork
    {
        private readonly string connectionString;

        public DataBaseWork()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinManage");
            
            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var dbpath = Path.Combine(folder, "Finmanage.db");

            connectionString = $"Data Source={dbpath}";

            InitialiseDatabaseLimits();
            InitialiseDatabaseMainFin();
        }

        private void InitialiseDatabaseLimits()
        {
            using (var connection = new SqliteConnection(connectionString)) 
            { 
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS Limits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT NOT NULL,
                    Amount REAL NOT NULL,
                    Currency TEXT NOT NULL,
                    Description TEXT
                );
                ";

                command.ExecuteNonQuery();
            }
        }

        private void InitialiseDatabaseMainFin()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open(); 

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                Create Table if not Exists MainData (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Category TEXT Not Null,
                    OperationType TEXT Not Null,
                    Name_of_Amount TEXT,
                    Amount REAL Not Null,
                    Currency TEXT Not Null,
                    DateInfo TEXT,
                    Description TEXT );
                ";

                command.ExecuteNonQuery();
            }
        }
        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
