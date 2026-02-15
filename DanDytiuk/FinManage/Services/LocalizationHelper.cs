using FinManage.Properties;
using System.ComponentModel;
using System.Globalization;
using System.Threading;

namespace FinManage.Services
{
    public class LocalizationHelper : INotifyPropertyChanged
    {
        private static LocalizationHelper _instance;
        public static LocalizationHelper Instance => _instance ?? (_instance = new LocalizationHelper());

        public event PropertyChangedEventHandler PropertyChanged;
        
        public string this[string key]
        {
            get => Resources.ResourceManager.GetString(key);
        }

        public void ChangeLang(string culurecode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culurecode);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
