using System;
using System.Windows.Navigation;

namespace Storm.Mvvm.Services
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
