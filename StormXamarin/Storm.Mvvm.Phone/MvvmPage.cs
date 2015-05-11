using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public class MvvmPage : PhoneApplicationPage
	{
		#region Protected methods

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			NavigationArgs args = NavigationHelper.FromArgs(e);
			ViewModelBase vm = DataContext as ViewModelBase;
			if(vm != null)
			{
				vm.OnNavigatedFrom(args);
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			NavigationArgs args = NavigationHelper.FromArgs(e);
			string parametersKey = NavigationContext.QueryString["key"];
			ViewModelBase vm = DataContext as ViewModelBase;
			if (vm != null)
			{
				vm.OnNavigatedTo(args, parametersKey);
			}
		}

		#endregion
	}
}
