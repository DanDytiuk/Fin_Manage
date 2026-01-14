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
        private Category _category;
        #endregion

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

        public Category Category
        {
            get => _category;
            set
            {
                if (_category != value) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        public LimitModel()
        {
            
        }
    }
}
