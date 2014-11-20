using System;
using System.Globalization;
using System.Windows.Data;

namespace TestApp.Android.Converters
{
	public class StringToUpperConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = value as string;
			if (string.IsNullOrEmpty(str))
			{
				return "";
			}
			return str.ToUpper(culture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
