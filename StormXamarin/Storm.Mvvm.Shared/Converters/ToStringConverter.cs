﻿using System;
#if WINDOWS_APP || WINDOWS_UWP
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
#else
using System.Windows.Data;
using System.Globalization;
#endif

namespace Storm.Mvvm.Converters
{
	public class ToStringConverter : IValueConverter
	{
#if WINDOWS_APP || WINDOWS_UWP
		public object Convert(object value, Type targetType, object parameter, string language)
#else
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
#endif
		{
			if (value == null)
			{
				return "(null)";
			}
			return value.ToString();
		}

#if WINDOWS_APP || WINDOWS_UWP
		public object ConvertBack(object value, Type targetType, object parameter, string language)
#else
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
#endif
		{
			throw new NotImplementedException();
		}
	}
}
