using FinManage.Services;
using FinManage.ViewModels.Base;
using System.Reflection.Emit;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models
{
    internal class OperationModel : BaseViewModel
    {
        public TypeOperation Type { get; set; }
        public string ResourceKey { get; set; }

        public string DisplayName => LocalizationHelper.Instance[ResourceKey];

        public void Refresh() => OnPropertyChanged(nameof(DisplayName));
    }
}
