using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Diagnostics;


namespace HojozatyCode.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            return value.ToString().Equals(parameter.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue && boolValue)
                return parameter.ToString();

            return null;
        }
    }
}