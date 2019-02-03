using System;
using Storm.Mvvm.Services;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public class MvvmApplication : Application
	{
		/// <summary>
		/// Initialize MvvmApplication with default service but do not initialize MainPage.
		/// You need to call InitializeMainPage next !
		/// </summary>
		/// <param name="serviceRegisterCallback">Callback to register additional services if needed.</param>
		internal MvvmApplication(Action serviceRegisterCallback = null)
		{
			DependencyService.Register<INavigationService, NavigationService>();
			DependencyService.Register<IDialogService, DialogService>();
			DependencyService.Register<ICurrentPageService, CurrentPageService>();

			serviceRegisterCallback?.Invoke();
		}

		/// <summary>
		/// Initialize MvvmApplication with default services and initialize MainPage if mainPageMaker is not null.
		/// </summary>
		/// <param name="mainPageMaker">Callback to create the MainPage once all services has been registered</param>
		/// <param name="serviceRegisterCallback">Callback to register additional services if needed.</param>
		public MvvmApplication(Func<Page> mainPageMaker, Action serviceRegisterCallback = null) : this(serviceRegisterCallback)
		{
			InitializeMainPage(mainPageMaker());
		}

		protected void InitializeMainPage<TMainPage>() where TMainPage : Page, new()
		{
			InitializeMainPage(new TMainPage());
		}

		protected void InitializeMainPage(Page mainPage)
		{
			DependencyService.Get<ICurrentPageService>().Push(mainPage);
			MainPage = new MvvmNavigationPage(mainPage);
		}
	}

	//VJU: commenté puisque le partial Xaml a du mal avec le type générique
	//public class MvvmApplication<TMainPage> : MvvmApplication where TMainPage : Page, new()
	//{
	//	public MvvmApplication(Action serviceRegisterCallback = null) : base(() => new TMainPage(), serviceRegisterCallback)
	//	{
			
	//	}
	//}
}
