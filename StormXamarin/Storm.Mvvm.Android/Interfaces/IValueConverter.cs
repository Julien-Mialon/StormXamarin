using System.Globalization;

// Namespace for compatibility issue only
// ReSharper disable once CheckNamespace
namespace System.Windows.Data
{
	public interface IValueConverter
	{
		object Convert(object value, Type targetType, object parameter, CultureInfo culture);
		object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
	}
}
