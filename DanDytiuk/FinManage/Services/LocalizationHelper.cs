using System.ComponentModel;
using System.Globalization;
using FinManage.Properties;

namespace FinManage.Services
{
    public class LocalizationHelper : INotifyPropertyChanged
    {
        private static LocalizationHelper _instance;
        public static LocalizationHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LocalizationHelper();

                return _instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string this[string key] =>
            Resources.ResourceManager.GetString(key, Resources.Culture);

        public void SetLanguage(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            Resources.Culture = culture;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}