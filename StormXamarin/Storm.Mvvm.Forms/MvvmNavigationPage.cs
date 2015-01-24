using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm.Mvvm.Patterns;
using Storm.Mvvm.Services;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public class MvvmNavigationPage : NavigationPage
	{
		public INavigationService NavigationService
		{
			get { return LazySingletonInitializer<INavigationService>.Value; }
		}

		public MvvmNavigationPage(Page root) : base(root)
		{
			this.Pushed += (sender, args) =>
			{
				NavigationService.OnPush(args.Page, NavigationMode.Push);
			};

			this.Popped += (sender, args) =>
			{
				NavigationService.OnPop(args.Page, NavigationMode.Push);
			};
		}
	}
}
