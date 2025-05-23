using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Maui.Controls;
using System.Diagnostics;


namespace HojozatyCode.Converters
{
	public class GenderToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
				return false;

			return value.ToString().Equals(parameter.ToString(), StringComparison.OrdinalIgnoreCase);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || parameter == null)
				return null;

			return (bool)value ? parameter.ToString() : null;
		}
	}
}