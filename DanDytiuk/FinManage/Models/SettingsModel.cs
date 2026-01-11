using System.Security.Policy;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models
{
    public class SettingsModel
    {
        public Themes Theme { get; set; } = Themes.Light;
        public TypesOfCurrency Currency { get; set; } = TypesOfCurrency.USD;
        public decimal? MonthlyLimit { get; set; } = 0;
        public TypeOperation LimitOfOperation { get; set; } = 0;
    }
}
