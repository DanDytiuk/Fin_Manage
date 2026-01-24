using System;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models.Models_for_db
{
    internal class FinAllTableModel
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string 
        public TypeOperation OperationType { get; set; }
        public string NameOfAmount { get; set; } = "";
        public decimal Amount { get; set; }
        public DateTime DateInfo { get; set; }
        public string Description { get; set; } = "";

    }
}
