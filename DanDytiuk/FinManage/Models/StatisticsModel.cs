namespace FinManage.Models
{
    internal class StatisticsModel
    {
        public string Category { get; set; }
        public string Currency { get; set; }

        public decimal MinAmount { get; set; } 
        public decimal AvgAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal Limit {  get; set; }

        public int ValueMonth { get; set; }
        public string NameMonth { get; set; }

        
    }
}
