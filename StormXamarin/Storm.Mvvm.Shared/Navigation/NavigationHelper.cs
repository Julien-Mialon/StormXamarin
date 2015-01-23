#if WINDOWS_APP || WINDOWS_PHONE
using System;
using Storm.Mvvm.Services;
#if WINDOWS_PHONE
using System.Windows.Navigation;
#else
using Windows.UI.Xaml.Navigation;
#endif

namespace Storm.Mvvm.Navigation
{
	internal static class NavigationHelper
	{
		public static NavigationArgs.NavigationMode FromMode(NavigationMode mode)
		{
			return (NavigationArgs.NavigationMode)Enum.Parse(typeof(NavigationArgs.NavigationMode), mode.ToString());
		}

		public static NavigationArgs FromArgs(NavigationEventArgs args)
		{
			return new NavigationArgs(FromMode(args.NavigationMode));
		}
	}
}

#endif