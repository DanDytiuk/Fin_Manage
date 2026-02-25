using System;
using System.Security.RightsManagement;
using System.Windows.Data;
using System.Windows.Markup;

namespace FinManage.Services
{
    [MarkupExtensionReturnType(typeof(string))]
    public class LocExtension : MarkupExtension
    {
        public string Key { get; set; }

        public LocExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(Key))
                return "[NoKey]";

            var helper = LocalizationHelper.Instance;
            if (helper == null)
                return "[NoHelper]";

            var value = helper[Key];
            return value ?? $"[{Key}]";
        }
    }
}
