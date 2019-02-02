using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Storm.Mvvm.Patterns;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class NavigationService : INavigationService
	{
		private readonly Stack<Tuple<Page, NavigationMode>> _pages = new Stack<Tuple<Page, NavigationMode>>();

		public event EventHandler<PagePushEventArgs> ViewPushed;
		public event EventHandler<PagePopEventArgs> ViewPopped;

		protected Page CurrentPage => LazySingletonInitializer<ICurrentPageService>.Value.CurrentPage;

		public Task PushAsync(Page page, Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true)
		{
			_pages.Push(new Tuple<Page, NavigationMode>(page, mode));
			
			if (page.BindingContext is ViewModelBase vm)
			{
				vm.Initialize(parameters ?? new Dictionary<string, object>());
			}

			Task result;
			if (mode == NavigationMode.Push)
			{
				result = CurrentPage.Navigation.PushAsync(page, animated);
				// Push will be handled by the MvvmNavigationPage
			}
			else
			{
				result = CurrentPage.Navigation.PushModalAsync(page, animated);
				OnPush(page, mode);
			}
			
			return result;
		}

		public Task PushAsync<TPage>(Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true) where TPage : Page, new()
		{
			return PushAsync(new TPage(), parameters, mode, animated);
		}

		public Task<Page> PopAsync(bool animated = true)
		{
			Tuple<Page, NavigationMode> top = _pages.Pop();
			Task<Page> result;
			if (top.Item2 == NavigationMode.Push)
			{
				result = CurrentPage.Navigation.PopAsync(animated);
				// Pop will be handled by the MvvmNavigationPage
			}
			else
			{
				result = CurrentPage.Navigation.PopModalAsync(animated);
				OnPop(top.Item1, top.Item2);
			}
			return result;
		}

		public void OnPush(Page page, NavigationMode mode)
		{
			ViewPushed?.Invoke(this, new PagePushEventArgs(page, mode));
		}

		public void OnPop(Page page, NavigationMode mode)
		{
			ViewPopped?.Invoke(this, new PagePopEventArgs(page, mode));
		}
	}
}
