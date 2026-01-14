using FinManage.ViewModels.Base;
using System.Text.Json.Serialization;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models 
{
    internal class SettingsModel : BaseViewModel
    {
        #region PropertyChangedValue
        private Themes _theme;
        private TypesOfCurrency _currency;
        private decimal _monthlylimit;
        private Category _limitoperation;
        #endregion
        #region ItemsSettings
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Themes Theme
        {
            get => _theme;
            set
            {
                if(value == _theme) return;
                _theme = value;
                OnPropertyChanged();
            }
        }
        public TypesOfCurrency Currency
        {
            get => _currency;
            set
            {
                if(value == _currency) return;
                _currency = value;
                OnPropertyChanged();
            }
        }
        public decimal? MonthlyLimit 
        {
            get => _monthlylimit;
            set
            {
                if(value == _monthlylimit) return;
                _monthlylimit = (decimal)value;
                OnPropertyChanged();
            }
        }
        public Category? LimitOfOperation
        {
            get => _limitoperation;
            set
            {
                if(value == _limitoperation) return;
                _limitoperation = (Category)value;
                OnPropertyChanged();
            }
        }
        #endregion
        public SettingsModel()
        {
            Theme = Themes.Light;
            MonthlyLimit = 0;
            Currency = TypesOfCurrency.USD;
        }
        
    }
}
