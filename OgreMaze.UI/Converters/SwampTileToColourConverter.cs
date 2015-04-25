using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using OgreMaze.Core;
using OgreMaze.Core.Enums;

namespace OgreMaze.UI.Converters
{
    public class SwampTileToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tileType = ((SwampTile) value).SwampTileType;

            switch (tileType)
            {
                case TileType.Empty:
                    return new SolidColorBrush(Colors.White);
                case TileType.Ogre:
                    return new SolidColorBrush(Colors.ForestGreen);
                case TileType.OgreFootprints:
                    return new SolidColorBrush(Colors.Gray);
                case TileType.Gold:
                    return new SolidColorBrush(Colors.Gold);
                case TileType.SinkHole:
                    return new SolidColorBrush(Colors.DarkRed);
            }

            return new SolidColorBrush(Colors.White);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
