using FinManage.Properties;
using FinManage.Services;
using System.ComponentModel;
using System.Threading;

namespace FinManage.Models
{
    /*internal class CategoryModel : INotifyPropertyChanged
    {
        public string ResourceKey { get; set; }

        public string DisplayName => Resources.ResourceManager.GetString(ResourceKey);
    
        public void Refresh()
        {
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
            OnPropertyChanged
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }*/

    internal class CategoryModel : INotifyPropertyChanged
    {
        public string ResourceKey { get; set; }

        //public string DisplayName => Resources.ResourceManager.GetString(ResourceKey);

        public string DisplayName =>
         LocalizationHelper.Instance[ResourceKey];

        public void Refresh()
        {
            OnPropertyChanged(nameof(DisplayName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
