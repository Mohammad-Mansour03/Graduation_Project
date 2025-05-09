using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace HojozatyCode.Converters
{
    public class BoolToImageConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isFavorite && parameter is string param)
            {
                string[] images = param.Split(',');
                if (images.Length >= 2)
                {
                    return isFavorite ? images[1] : images[0];
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}