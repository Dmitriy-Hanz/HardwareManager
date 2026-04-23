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
    public class AccessLvlVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.GetType().Name)
            {
                case "AccessLevel":
                    string[] privileges = parameter.ToString().Split(' ');
                    return privileges.Contains(value.ToString()) ? Visibility.Visible : Visibility.Collapsed;
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)value;
        }
    }
}
