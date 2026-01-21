using FinManage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

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

        public void AddOrUpdateCategory(string category, decimal amount)
        {
            int categoryID = _categories.Add(category);

            using (var connection = _dataBaseWork.GetConnection())
            {

            }
        }
    }
}
