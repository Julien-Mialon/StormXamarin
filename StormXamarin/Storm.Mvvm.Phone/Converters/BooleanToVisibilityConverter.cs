using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Storm.Mvvm.Converters
{
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool invert = (parameter is bool) && (bool)parameter;
			bool val = (bool)value;

			return (val ^ invert) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool invert = (parameter is bool) && (bool)parameter;
			Visibility val = (Visibility)value;
			bool booleanValue = (val == Visibility.Visible);

			return (booleanValue ^ invert);
		}
	}
}
