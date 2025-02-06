//using System;
//using System.Globalization;
//using System.Windows;

//namespace HojozatyCode.Converters
//{
 

//    public class BooleanToImageConverter : IValueConverter
//    {
//        //Changes the eye icon image based on the IsVisiblePassword
//        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
//        {
//            if (value is bool boolValue && parameter is string param)
//            {
//                //split the parameter into two image names
//                var images = param.Split(',');
//                //select the image based on the boolean value
//                return boolValue ? images[0] : images[1];
//            }
//            return null;//return null if the value is invalid
//        }

//        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
//        {
//            throw new NotImplementedException();//isn't used
//        }
//    }
//}