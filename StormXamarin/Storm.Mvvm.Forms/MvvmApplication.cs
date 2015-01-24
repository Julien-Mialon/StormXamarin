using System;
using Storm.Mvvm.Services;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public class MvvmApplication<TMainPage> : Application where TMainPage : Page, new()
	{
		public MvvmApplication(Action serviceRegisterCallback = null)
		{
			DependencyService.Register<INavigationService, NavigationService>();
			DependencyService.Register<IDialogService, DialogService>();
			DependencyService.Register<ICurrentPageService, CurrentPageService>();

			if (serviceRegisterCallback != null)
			{
				serviceRegisterCallback();
			}

			Page mainPage = new TMainPage();

			DependencyService.Get<ICurrentPageService>().Push(mainPage);
			
			MainPage = new NavigationPage(mainPage);
		}
	}
}
