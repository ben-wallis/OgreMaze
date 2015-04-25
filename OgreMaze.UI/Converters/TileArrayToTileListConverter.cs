﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using OgreMaze.Core;

namespace OgreMaze.UI.Converters
{
    public class TileArrayToTileListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var retList = new List<List<SwampTile>>();

            var array = (SwampTile[,])value;

            for (var y = 0; y < array.GetLength(1); y++)
            {
                var innerList = new List<SwampTile>();
                for (var x = 0; x < array.GetLength(0); x++)
                {
                    innerList.Add(array[x, y]);
                }
                retList.Add(innerList);
            }

            return retList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
