using System;
using Storm.Mvvm.Services;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public class MvvmApplication : Application
	{
		public MvvmApplication(Func<Page> mainPageMaker, Action serviceRegisterCallback = null)
		{
			DependencyService.Register<INavigationService, NavigationService>();
			DependencyService.Register<IDialogService, DialogService>();
			DependencyService.Register<ICurrentPageService, CurrentPageService>();

			if (serviceRegisterCallback != null)
			{
				serviceRegisterCallback();
			}

			Page mainPage = mainPageMaker();

			DependencyService.Get<ICurrentPageService>().Push(mainPage);

			MainPage = new MvvmNavigationPage(mainPage);
		}
	}

	public class MvvmApplication<TMainPage> : MvvmApplication where TMainPage : Page, new()
	{
		public MvvmApplication(Action serviceRegisterCallback = null) : base(() => new TMainPage(), serviceRegisterCallback)
		{
			
		}
	}
}
