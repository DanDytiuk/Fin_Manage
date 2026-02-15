using System;
using System.Globalization;
using System.Windows.Data;

namespace FinManage.Services
{
    internal class EnumLocalizationHelper : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return LocalizationHelper.Instance[value.ToString()];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
