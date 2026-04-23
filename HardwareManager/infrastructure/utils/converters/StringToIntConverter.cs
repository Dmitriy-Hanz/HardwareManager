using HardwareManager.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace HardwareManager.infrastructure.utils.converters
{
    class StringToIntConverter : MarkupExtension, IValueConverter
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
            if (int.TryParse(value.ToString(), out int r))
            {
                return r;
            }
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
