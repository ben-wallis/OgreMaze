using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OgreMaze.UI.Converters
{
    public class BooleanToShrekConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = new BitmapImage();
            image.BeginInit();
            
            if ((bool) value)
            {
                image.UriSource = new Uri("pack://application:,,,/OgreMaze.UI;component/Tileset/shrekhappy.png");
            }
            else
            {
                image.UriSource = new Uri("pack://application:,,,/OgreMaze.UI;component/Tileset/shreksad.png");
            }

            image.EndInit();
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
