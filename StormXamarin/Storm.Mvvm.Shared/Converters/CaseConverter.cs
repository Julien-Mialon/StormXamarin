using System;
#if WINDOWS_APP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace Storm.Mvvm.Converters
{
	public class CaseConverter : IValueConverter
	{
#if WINDOWS_APP
		public object Convert(object value, Type targetType, object parameter, string language)
#else
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
		{
			bool upper = parameter as bool? ?? true;
			string val = value as string ?? "";
			return upper ? val.ToUpperInvariant() : val.ToLowerInvariant	()

			string format = (parameter as string) ?? string.Empty;
			return string.Format(format, value);
		}

#if WINDOWS_APP
		public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
		{
			throw new NotImplementedException();
		}
	}
}
