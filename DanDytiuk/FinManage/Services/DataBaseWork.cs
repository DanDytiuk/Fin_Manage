using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace FinManage.Services
{
    internal class DataBaseWork
    {
        private readonly string connectionString;

        public DataBaseWork()
        {
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FinManage");
            
            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);

            var dbpath = Path.Combine(folder, "Finmange.db");

            connectionString = $"Data Source={dbpath}";

            InitialiseDatabase();
        }

        private void InitialiseDatabase()
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
                Amount REAL NOT NULL
                );
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
