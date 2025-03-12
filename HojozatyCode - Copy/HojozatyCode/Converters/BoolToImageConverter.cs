using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace HojozatyCode.Converters
{
    public class BoolToImageConverter : IValueConverter
    {
        //Depending on if you select the checkbox or not convert between check boxes
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ObservableCollection<string> collection && parameter is string spaceType)
            {
                //Check if that checkbox is selected
                bool isSelected = collection.Contains(spaceType);
             
             //   Debug.WriteLine($"Space type: {spaceType}, IsSelected: {isSelected}");
                //What is the type of the check box will added in the image button
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