using Storm.Mvvm.Patterns;
using Storm.Mvvm.Services;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public class MvvmNavigationPage : NavigationPage
	{
		public INavigationService NavigationService => LazySingletonInitializer<INavigationService>.Value;

		public MvvmNavigationPage(Page root) : base(root)
		{
			Pushed += (sender, args) =>
			{
				NavigationService.OnPush(args.Page, NavigationMode.Push);
			};

			Popped += (sender, args) =>
			{
				NavigationService.OnPop(args.Page, NavigationMode.Push);
			};
		}
	}
}
