using System;
using System.Windows.Navigation;

namespace Storm.Mvvm
{
	internal static class NavigationHelper
	{
		public static NavigationArgs.NavigationMode FromMode(NavigationMode _mode)
		{
			return (NavigationArgs.NavigationMode)Enum.Parse(typeof(NavigationArgs.NavigationMode), _mode.ToString());
		}

		public static NavigationArgs FromArgs(NavigationEventArgs _args)
		{
			return new NavigationArgs(FromMode(_args.NavigationMode));
		}
	}
}
