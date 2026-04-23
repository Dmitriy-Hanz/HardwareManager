using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
namespace HardwareManager.infrastructure.utils.converters
{
    public class MultiValueVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static MultiValueVisibilityConverter multiValueConverter = null;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => multiValueConverter ?? new MultiValueVisibilityConverter();

    }
}
