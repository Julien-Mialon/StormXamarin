using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm
{
	public class MvvmPage : Page
	{
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			ViewModelBase viewModel = DataContext as ViewModelBase;
			if (viewModel != null)
			{
				viewModel.OnNavigatedFrom(NavigationHelper.FromArgs(e));
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			string parameterKey = e.Parameter as string;
			ViewModelBase viewModel = DataContext as ViewModelBase;
			if (viewModel != null && parameterKey != null)
			{
				viewModel.OnNavigatedTo(NavigationHelper.FromArgs(e), parameterKey);
			}
		}
	}
}
