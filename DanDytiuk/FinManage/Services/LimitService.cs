using System;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Services
{
    internal class LimitService
    {
        private readonly DataBaseWork _database;

        public LimitService(DataBaseWork database) 
        { 
            _database = database;
        }

        public LimitCheckResult CheckLimit(DateTime date, string category)
        {
            using (var connection = _database.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"SELECT LimitAmount, DateFrom, DateTo
              FROM Limits
              WHERE Category = @category
              AND @date BETWEEN DateFrom AND DateTo";

                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@date", date);

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                        return LimitCheckResult.NoLimit;

                    decimal limit = reader.GetDecimal(0);
                    DateTime from = reader.GetDateTime(1);
                    DateTime to = reader.GetDateTime(2);

                    decimal total = GetTotalExpenses(category, from, to);

                    if (total > limit)
                        return LimitCheckResult.OverLimit;

                    if (DateTime.Now > to)
                        return LimitCheckResult.Expired;

                    return LimitCheckResult.Ok;
                }
            }
        }

        private decimal GetTotalExpenses(string category, DateTime from, DateTime to)
        {
            using (var connection = _database.GetConnection())
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText =
                @"SELECT IFNULL(SUM(Amount),0)
              FROM MainData
              WHERE Category = @category
              AND DateInfo BETWEEN @from AND @to";

                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);

                return Convert.ToDecimal(command.ExecuteScalar());
            }
        }
    }
}
