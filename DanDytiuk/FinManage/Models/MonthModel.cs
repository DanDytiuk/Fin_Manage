using FinManage.Services;
using FinManage.ViewModels.Base;

namespace FinManage.Models
{
    internal class MonthModel : BaseViewModel
    {
        public int ValueMonth { get; set; }
        public string ResourceKey { get; set; }

        public string DisplayName => LocalizationHelper.Instance[ResourceKey];

        public void Refresh() => OnPropertyChanged(nameof(DisplayName));
    }
}
