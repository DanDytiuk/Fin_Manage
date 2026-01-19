using FinManage.ViewModels.Base;
using System.Collections.ObjectModel;

namespace FinManage.Models
{
    internal class LimitModel : BaseViewModel
    {
        private string _category;
        private decimal _monthlyLimit;
        public string Category 
        {
            get => _category;
            set => Set(ref _category, value);
        }
        public decimal MonthlyLimit 
        {
            get => _monthlyLimit;
            set => Set(ref _monthlyLimit, value);
        }
    }
}
