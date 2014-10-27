using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Storm.Framework.Converters
{
	public sealed class BooleanToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool invert = (parameter != null && parameter is bool) ? (bool)parameter : false;
			bool val = (bool)value;

			return (val ^ invert) ? Visibility.Visible : Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool invert = (parameter != null && parameter is bool) ? (bool)parameter : false;
			Visibility val = (Visibility)value;
			bool booleanValue = (val == Visibility.Visible);

			return (booleanValue ^ invert) ? true : false;
		}
	}
}
