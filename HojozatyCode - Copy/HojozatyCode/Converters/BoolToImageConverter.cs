using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace HojozatyCode.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> collection && parameter is string spaceType)
            {
                bool isSelected = collection.Contains(spaceType);
                Debug.WriteLine($"Space type: {spaceType}, IsSelected: {isSelected}");
                return isSelected ? "checkbox.png" : "checkbox_unchecked.png";
            }
            
            return "checkbox_unchecked.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}