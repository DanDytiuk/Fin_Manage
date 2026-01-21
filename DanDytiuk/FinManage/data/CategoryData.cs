using System;
using System.Collections.Generic;
using FinManage.Models;
using FinManage.Models.Models_for_db;
using FinManage.Services;
using Microsoft.Data.Sqlite;

namespace FinManage.Data
{
    internal class CategoryData
    {
        private readonly DataBaseWork _dataBaseWork;

        public CategoryData(DataBaseWork dataBaseWork)
        {
            _dataBaseWork = dataBaseWork;
        }

        public int Add(string categoryName)
        {
            using (var connection = _dataBaseWork.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
                    INSERT OR IGNORE INTO Categories (Name)
                    VALUES ($name);
                    SELECT Id FROM Categories WHERE Name = $name;
                    ";

                    command.Parameters.AddWithValue("$name", categoryName);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public List<CategoryModel> GetAll()
        {
            var list = new List<CategoryModel>();

            using (var connection = _dataBaseWork.GetConnection()) 
            {
                connection.Open();

                using (var command = connection.CreateCommand()) 
                {
                    command.CommandText = "SELECT Id, Name FROM Categories";

                    using (var reader = command.ExecuteReader()) 
                    {
                        while (reader.Read())
                        {
                            list.Add(new CategoryModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
