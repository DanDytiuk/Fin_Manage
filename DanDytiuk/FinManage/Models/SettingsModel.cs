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
        
        #endregion
        public SettingsModel()
        {
            Theme = Themes.Light;
            Currency = TypesOfCurrency.USD;
        }
        
    }
}
