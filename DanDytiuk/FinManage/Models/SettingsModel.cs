using System.Security.Policy;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Models
{
    public class SettingsModel
    {
        public string Theme { get; set; }
        public TypesOfCurrency Currency { get; set; }
        public decimal MonthlyLimit { get; set; }

    }
}
