using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace TestApp.Android.Converters
{
	public class IntToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			uint color = (uint) value;

			return new ColorDrawable(new Color((int) color));
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
