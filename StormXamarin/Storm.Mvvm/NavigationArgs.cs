using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm
{
	public class NavigationArgs
	{
		public enum NavigationMode
		{
			Back,
			Forward,
			New,
			Refresh,
			Reset,
		}

		public NavigationMode Mode { get; private set; }

		public NavigationArgs(NavigationMode _mode)
		{
			this.Mode = _mode;
		}
	}
}
