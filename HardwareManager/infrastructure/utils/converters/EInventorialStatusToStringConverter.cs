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
    class EInventorialStatusToStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
            {
                return "";
            }
            return (EInventorialStatus)value switch
            {
                EInventorialStatus.InWork => "В работе",
                EInventorialStatus.InStock => "На складе",
                EInventorialStatus.Removed => "Списан",
                _ => ""
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value switch
            {
                "В работе" => EInventorialStatus.InWork,
                "На складе" => EInventorialStatus.InStock,
                "Списан" => EInventorialStatus.Removed,
                _ => 0
            };
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
