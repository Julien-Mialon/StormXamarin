using System;
using System.Globalization;

namespace TestApp.Android.Converters
{
	public class StringToUpperConverter : System.Windows.Data.IValueConverter
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
