using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace HardwareManager.infrastructure.utils.converters
{
    class StringToDoubleConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !value.Equals(""))
            {
                return value.ToString();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || value.Equals(""))
            {
                return null;
            }
            if (double.TryParse(value.ToString(), out double r))
            {
                return r;
            }
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
