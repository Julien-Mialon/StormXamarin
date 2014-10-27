using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm
{
	internal static class NavigationHelper
	{
		public static NavigationArgs.NavigationMode FromMode(System.Windows.Navigation.NavigationMode _mode)
		{
			return (NavigationArgs.NavigationMode)Enum.Parse(typeof(NavigationArgs.NavigationMode), _mode.ToString());
		}

		public static NavigationArgs FromArgs(System.Windows.Navigation.NavigationEventArgs _args)
		{
			return new NavigationArgs(NavigationHelper.FromMode(_args.NavigationMode));
		}
	}
}
