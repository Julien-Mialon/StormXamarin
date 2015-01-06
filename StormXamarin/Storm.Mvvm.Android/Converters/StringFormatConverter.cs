using System;
using System.Globalization;
using System.Windows.Data;

namespace Storm.Mvvm.Converters
{
	public class StringFormatConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string format = (parameter as string) ?? string.Empty;
			return string.Format(format, value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
