using FinManage.ViewModels.Base;
using System;
using System.Collections.Generic;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models
{
    internal class LimitModel : BaseViewModel
    {
        #region PropertyChangedValue

        private decimal AmountMonthlyLimitCB;
        private Category _category;
        private Category SelectCategory;
        private decimal FillAmount;
        private LimitModel _selectedlimit;

        #endregion
        public decimal Monthlylimit
        {
            get => AmountMonthlyLimitCB;
            set
            {
                if (AmountMonthlyLimitCB == value) return;
                AmountMonthlyLimitCB = value;
                OnPropertyChanged();
            }
        }

        public Category Category
        {
            get => _category;
            set
            {
                if (_category == value) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        public Category SelectCategoryFromUser
        {
            get => SelectCategory;
            set
            {
                if (SelectCategory == value) return;
                SelectCategory = value;
                OnPropertyChanged();
            }
        }

        public decimal? FillAmountFromUser
        {
            get => FillAmount;
            set
            {
                if (FillAmount == value) return;
                FillAmount = (decimal)value;
                OnPropertyChanged();
            }
        }

        public LimitModel Selectedlimit
        {
            get => _selectedlimit;
            set
            {
                if (_selectedlimit == value) return;
                _selectedlimit = value;
                OnPropertyChanged();
            }
        }
    }
}
