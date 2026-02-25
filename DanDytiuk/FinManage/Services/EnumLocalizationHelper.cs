using FinManage.Properties;
using System;
using System.Globalization;
using System.Windows.Data;

namespace FinManage.Services
{
    public static class EnumLocalizationHelper
    {
        public static string GetLocalized<T>(T enumValue) where T : Enum
        {
            string key = $"{typeof(T).Name}_{enumValue}";

            string localized = Resources.ResourceManager.GetString(key, Resources.Culture);

            return string.IsNullOrEmpty(localized)
                ? enumValue.ToString()
                : localized;
        }
    }
}
