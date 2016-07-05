using System;
#if WINDOWS_APP || WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace Storm.Mvvm.Converters
{
    public class BooleanNegationConverter : IValueConverter
    {
#if WINDOWS_APP || WINDOWS_UWP
		public object Convert(object value, Type targetType, object parameter, string language)
#else
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
	    {
		    bool invert = true;

		    if (parameter != null)
		    {
			    if (parameter is string)
			    {
				    invert = bool.Parse((string) parameter);
			    }
				else if (parameter is bool)
				{
					invert = (bool)parameter;
				}
		    }

		    bool val = (bool) value;
		    return invert ? val : !val;
	    }

#if WINDOWS_APP || WINDOWS_UWP
		public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
	    {
			bool invert = true;

			if (parameter != null)
			{
				if (parameter is string)
				{
					invert = bool.Parse(parameter as string);
				}
				else if (parameter is bool)
				{
					invert = (bool)parameter;
				}
			}

			bool val = (bool)value;
			return invert ? val : !val;
	    }
    }
}
