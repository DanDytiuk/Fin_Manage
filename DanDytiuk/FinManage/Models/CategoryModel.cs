using FinManage.Properties;
using FinManage.Services;
using System.ComponentModel;
using System.Threading;

namespace FinManage.Models
{
    internal class CategoryModel : INotifyPropertyChanged
    {
        public string ResourceKey { get; set; }

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
