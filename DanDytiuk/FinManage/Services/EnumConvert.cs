using System;
using System.Globalization;
using System.Windows.Data;

namespace FinManage.Services
{
    internal class EnumConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return null; }

            return EnumLocalizationHelper.GetLocalized((Enum)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
