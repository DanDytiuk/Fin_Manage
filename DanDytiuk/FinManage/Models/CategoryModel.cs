using FinManage.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinManage.Models
{
    internal class CategoryModel : INotifyPropertyChanged
    {
        public string ResourceKey { get; set; }

        public string DisplayName => Resources.ResourceManager.GetString(ResourceKey);
    
        public void Refresh()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DisplayName)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
