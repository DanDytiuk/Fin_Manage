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
        private string _language;

        #endregion

        #region ItemsSettings
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Themes Theme
        {
            get => _theme;
            set => Set(ref _theme, value);
        }
        public TypesOfCurrency Currency
        {
            get => _currency;
            set => Set(ref _currency, value);
        }
        public string Language 
        {
            get => _language;
            set => Set(ref _language, value);
        }
        #endregion
        public SettingsModel()
        {
            Theme = Themes.Light;
            Currency = TypesOfCurrency.USD;
            Language = "en";
        }
        
    }
}
