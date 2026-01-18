using FinManage.ViewModels.Base;
using System;
using System.Collections.Generic;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models
{
    internal class LimitModel : BaseViewModel
    {
        #region PropertyChangedValue

        private decimal AmountMonthlyLimit;
        private Category SelectCategory;

        #endregion
        public decimal Monthlylimit
        {
            get => AmountMonthlyLimit;
            set
            {
                if (AmountMonthlyLimit != value) return;
                AmountMonthlyLimit = value;
                OnPropertyChanged();
            }
        }

        public Category SelectedCategory
        {
            get => SelectCategory;
            set
            {
                if (SelectCategory != value) return;
                SelectCategory = value;
                OnPropertyChanged();
            }
        }

    }
}
