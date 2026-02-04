using FinManage.Models.Models_for_db;
using FinManage.Services;
using System;
using System.Collections.Generic;

namespace FinManage.Data
{
    internal class LimitData
    {
        private readonly DataBaseWork _dataBaseWork;
        private readonly CategoryData _categories;

        public LimitData(DataBaseWork dataBaseWork)
        {
            _dataBaseWork = dataBaseWork;
            _categories = new CategoryData(dataBaseWork);
        }

        public void AddOrUpdateInfo(string category, decimal amount)
        {
            int categoryID = _categories.Add(category);

            using (var connection = _dataBaseWork.GetConnection())
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText =
                    @"
                    INSERT INTO Limits (CategoryId, Amount)
                    VALUES ($categoryId, $amount)
                    ON CONFLICT(CategoryId)
                    DO UPDATE SET Amount = $amount;
                    ";

                    cmd.Parameters.AddWithValue("$categoryId", categoryID);
                    cmd.Parameters.AddWithValue("$amount", amount);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /*public void Delete(int limitId)
        {
            using (var connection = _dataBaseWork.GetConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Limits WHERE Id = $id";
                    command.Parameters.AddWithValue("$id", limitId);
                    command.ExecuteNonQuery();
                }
            }
        }*/
    }
}
