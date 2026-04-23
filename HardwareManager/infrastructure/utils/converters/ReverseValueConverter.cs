using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
namespace HardwareManager.infrastructure.utils.converters
{
    public class ReverseValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Visibility:
                    return (Visibility)value == Visibility.Hidden || (Visibility)value == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                case bool:
                    return !(bool)value;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                case Visibility:
                    return (Visibility)value == Visibility.Hidden || (Visibility)value == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
                case bool:
                    return !(bool)value;
                default:
                    return null;
            }
        }
    }
}
