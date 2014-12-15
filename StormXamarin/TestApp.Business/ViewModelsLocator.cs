using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using TestApp.Business.ViewModels;

namespace TestApp.Business
{
	public class ViewModelsLocator
	{
		private static IContainer _container;

		public static void Initialize(IContainer container)
		{
			container.RegisterFactory(x => new MainPageViewModel(x));
			container.RegisterFactory(x => new SecondPageViewModel(x));
			container.RegisterFactory(x => new MainFragmentViewModel(x));
			_container = container;
		}

		public MainPageViewModel MainPageViewModel
		{
			get { return _container.Resolve<MainPageViewModel>(); }
		}

		public SecondPageViewModel SecondPageViewModel
		{
			get { return _container.Resolve<SecondPageViewModel>(); }
		}

		public MainFragmentViewModel MainFragmentViewModel
		{
			get { return _container.Resolve<MainFragmentViewModel>(); }
		}
	}
}
