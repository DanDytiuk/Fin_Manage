using FinManage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinManage.Models.Models_for_db
{
    internal class LimitsModel
    {
        public int Id { get; set; }
        public string CategoryKey { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }

        public string CategoryDisplay => LocalizationHelper.Instance[CategoryKey];

    }
}
