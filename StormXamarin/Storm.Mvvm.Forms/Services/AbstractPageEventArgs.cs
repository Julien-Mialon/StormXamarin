using System;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class AbstractPageEventArgs : EventArgs
	{
		public Page Page { get; private set; }

		public NavigationMode Mode { get; private set; }

		public AbstractPageEventArgs()
		{
			
		}

		public AbstractPageEventArgs(Page page, NavigationMode mode)
		{
			Page = page;
			Mode = mode;
		}
	}
}