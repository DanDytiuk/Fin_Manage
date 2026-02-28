using FinManage.Properties;
using System.ComponentModel;
using System.Globalization;

namespace FinManage.Models
{
    internal class StatisticsModel
    {
        public string Category { get; set; }
        public string Currency { get; set; }

        public string CategoryLocalize
        {
            get
            {
                return Resources.ResourceManager.GetString(
                    Category,
                    CultureInfo.CurrentUICulture) ?? Category;
            }
        }

        public decimal MinAmount { get; set; } 
        public decimal AvgAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal Limit {  get; set; }

        public int ValueMonth { get; set; }
        public string NameMonth { get; set; }

        public string SelectedCurrency { get; set; }
        public string SelectedMonth { get; set; }
        public string SelectedYear { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RefreshLocalizationCategory()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CategoryLocalize)));
        }
    }
}
