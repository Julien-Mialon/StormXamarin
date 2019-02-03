using System.Collections.Generic;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class CurrentPageService: ICurrentPageService
	{
		private readonly Stack<Page> _pages = new Stack<Page>();

		public Page CurrentPage => _pages.Peek();

		public CurrentPageService()
		{
			INavigationService navigationService = DependencyService.Get<INavigationService>();
			navigationService.ViewPushed += (sender, args) =>
			{
				Push(args.Page);
			};
			navigationService.ViewPopped += (sender, args) =>
			{
				Pop();
			};
		}

		public void Push(Page newPage) => _pages.Push(newPage);

		public void Pop() => _pages.Pop();
	}
}
