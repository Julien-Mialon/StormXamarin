using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class PagePushEventArgs : AbstractPageEventArgs
	{
		public PagePushEventArgs() : base()
		{
			
		}

		public PagePushEventArgs(Page page, NavigationMode mode) : base(page, mode)
		{
			
		}
	}
}