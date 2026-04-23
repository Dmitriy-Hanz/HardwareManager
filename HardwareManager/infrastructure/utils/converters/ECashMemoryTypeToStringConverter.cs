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
    class ECashMemoryTypeToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
            {
                return "";
            }

            return (ECashMemoryType)value switch
            {
                ECashMemoryType.L1 => "L1",
                ECashMemoryType.L2 => "L2",
                ECashMemoryType.L3 => "L3",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value switch
            {
                "L1" => ECashMemoryType.L1,
                "L2" => ECashMemoryType.L2,
                "L3" => ECashMemoryType.L3,
                _ => 0
            };
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
