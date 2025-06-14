﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls; // Make sure you're using this for .NET MAUI

namespace HojozatyCode.Converters // <- Add this
{
    public class ListContainsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var list = value as ObservableCollection<string>;
            var item = parameter as string;
            return list != null && list.Contains(item);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); // Not used
        }
    }
}
