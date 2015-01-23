using System.Collections.Generic;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public static class PageHelper
	{
		public static Page Get<TPage>(Dictionary<string, object> navigationParameters = null) where TPage : Page, new()
		{
			Page view = new TPage();
			ViewModelBase vm = view.BindingContext as ViewModelBase;

			if (vm != null)
			{
				vm.NavigationService = view.Navigation;
				vm.Initialize(navigationParameters ?? new Dictionary<string, object>());
			}

			return view;
		}
	}
}
