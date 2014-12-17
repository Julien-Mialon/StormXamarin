﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace TestApp.Android.Converters
{
	public class ToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
