using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		protected Page CurrentPage
		{
			get { return LazySingletonInitializer<ICurrentPageService>.Value.CurrentPage; }
		}

		public Task PushAsync(Page page, Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true)
		{
			_pages.Push(new Tuple<Page, NavigationMode>(page, mode));

			ViewModelBase vm = page.BindingContext as ViewModelBase;
			if (vm != null)
			{
				vm.Initialize(parameters ?? new Dictionary<string, object>());
			}

			Task result = (mode == NavigationMode.Push) ?
						CurrentPage.Navigation.PushAsync(page, animated) :
						CurrentPage.Navigation.PushModalAsync(page, animated);
			OnPush(page, mode);
			return result;
		}

		public Task PushAsync<TPage>(Dictionary<string, object> parameters = null, NavigationMode mode = NavigationMode.Push, bool animated = true) where TPage : Page, new()
		{
			return PushAsync(new TPage(), parameters, mode, animated);
		}

		public Task<Page> PopAsync(bool animated = true)
		{
			Tuple<Page, NavigationMode> top = _pages.Pop();
			Task<Page> result = (top.Item2 == NavigationMode.Push) ?
							CurrentPage.Navigation.PopAsync(animated) :
							CurrentPage.Navigation.PopModalAsync(animated);
			OnPop(top.Item1, top.Item2);
			return result;
		}

		protected void OnPush(Page page, NavigationMode mode)
		{
			var handler = ViewPushed;
			if (handler != null)
			{
				handler(this, new PagePushEventArgs(page, mode));
			}
		}

		protected void OnPop(Page page, NavigationMode mode)
		{
			var handler = ViewPopped;
			if (handler != null)
			{
				handler(this, new PagePopEventArgs(page, mode));
			}
		}
	}
}
