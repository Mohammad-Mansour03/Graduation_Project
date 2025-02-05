using System;
using System.Globalization;
using System.Windows;

namespace HojozatyCode.Converters
{
    public class BooleanToImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool boolValue && parameter is string param)
            {
                var images = param.Split(',');
                return boolValue ? images[0] : images[1];
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}