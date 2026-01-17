using FinManage.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models
{
    internal class LimitModel : BaseViewModel
    {
        #region PropertyChangedValue
        private decimal _monthlylimit;
        private Category _selectedCategory;
        public Array Categories => Enum.GetValues(typeof(Category));
        #endregion
        public Dictionary<Category, decimal> Limits {  get; set; } = new Dictionary<Category, decimal>();
        public decimal Monthlylimit
        {
            get => _monthlylimit;
            set
            {
                if (_monthlylimit != value) return;
                _monthlylimit = value;
                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value) return;
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

    }
}
