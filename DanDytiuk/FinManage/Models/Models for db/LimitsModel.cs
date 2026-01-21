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
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }
}
