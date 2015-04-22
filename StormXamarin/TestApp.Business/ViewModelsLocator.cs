using Storm.Mvvm.Inject;
using TestApp.Business.ViewModels;

namespace TestApp.Business
{
	public class ViewModelsLocator
	{
		private static IContainer _container;

		public static void Initialize(IContainer container)
		{
			container.RegisterFactory(x => new MainPageViewModel());
			container.RegisterFactory(x => new SecondPageViewModel());
			container.RegisterFactory(x => new MainFragmentViewModel());
			container.RegisterFactory(x => new AdapterViewModel());
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

		public AdapterViewModel AdapterViewModel
		{
			get { return _container.Resolve<AdapterViewModel>(); }
		}
	}
}
