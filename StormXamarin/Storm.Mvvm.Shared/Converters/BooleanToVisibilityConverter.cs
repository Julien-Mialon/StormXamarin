#if WINDOWS_APP || WINDOWS_PHONE
using System;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Data;
using System.Globalization;
#elif WINDOWS_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#endif

namespace Storm.Mvvm.Converters
{
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
#if WINDOWS_APP
		public object Convert(object value, Type targetType, object parameter, string language)
#else
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
		{
			bool invert = (parameter is bool) && (bool)parameter;
			bool val = (bool)value;

			return (val ^ invert) ? Visibility.Visible : Visibility.Collapsed;
		}

#if WINDOWS_APP
		public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
		{
			bool invert = (parameter is bool) && (bool)parameter;
			Visibility val = (Visibility)value;
			bool booleanValue = (val == Visibility.Visible);

			return (booleanValue ^ invert);
		}
	}
}

#endif