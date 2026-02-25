using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using static FinManage.Infrastructure.EnumInfrastructure;

namespace FinManage.Services
{
    internal class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TypeOperation currentType && parameter is string parameterString)
            {
                if (Enum.TryParse<TypeOperation>(parameterString, out var targetOperation))
                {
                    return currentType == targetOperation
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
