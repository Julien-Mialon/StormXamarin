using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace Storm.Mvvm
{
	public class PageView : PhoneApplicationPage
	{
		#region Constructors

		public PageView()
		{
			Microsoft.Phone.Shell.SystemTray.SetIsVisible(this, false);
			this.SupportedOrientations = SupportedPageOrientation.Portrait;
			this.Orientation = PageOrientation.Portrait;
		}

		#endregion

		#region Protected methods

		protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			NavigationArgs args = NavigationHelper.FromArgs(e);
			ViewModelBase vm = this.DataContext as ViewModelBase;
			if(vm != null)
			{
				vm.OnNavigatedFrom(args);
			}
		}

		protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			NavigationArgs args = NavigationHelper.FromArgs(e);
			ViewModelBase vm = this.DataContext as ViewModelBase;
			if (vm != null)
			{
				vm.OnNavigatedTo(args);
			}
		}

		#endregion
	}
}
