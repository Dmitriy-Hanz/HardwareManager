using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace HardwareManager.infrastructure.utils.converters
{
    public class BytesToImageConverter : MarkupExtension, IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            BitmapImage bitmapImage = new();

            using (MemoryStream memoryStream = new(value as byte[]))
            {
                memoryStream.Position = 0;
                bitmapImage.BeginInit();
                bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = null;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();
            }
            bitmapImage.Freeze();
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => this;

    }
}
