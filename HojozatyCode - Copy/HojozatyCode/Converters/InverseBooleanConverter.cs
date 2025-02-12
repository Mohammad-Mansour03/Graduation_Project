using System;
using System.Globalization;
using System.Windows;

namespace HojozatyCode.Converters
{

   public class InverseBooleanConverter : IValueConverter
   {
		//Converter inverts the boolean value (true becomes false and false beomes true)
		//used for the IsPassword property of the Entry field
		public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
       {
           if (value is bool boolValue)//Check if the value is boolean
           {
               return !boolValue;//Return the opposite value
           }
           return false;//Default to false if the value is not boolean
       }

       public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
       {
           throw new NotImplementedException();//isn't used 
       }
   }
}