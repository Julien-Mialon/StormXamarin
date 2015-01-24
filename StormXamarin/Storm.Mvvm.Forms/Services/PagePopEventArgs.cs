using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class PagePopEventArgs : AbstractPageEventArgs
	{
		public PagePopEventArgs() : base()
		{
			
		}

		public PagePopEventArgs(Page page, NavigationMode mode) : base(page, mode)
		{
			
		}
	}
}