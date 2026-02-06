using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinManage.Models
{
    internal class StatisticsModel
    {
        public string Category { get; set; }

        public decimal MinAmount { get; set; } 
        public decimal AvgAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal Limit {  get; set; }
    }
}
