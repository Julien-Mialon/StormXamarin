using Storm.Mvvm.Inject;

namespace TestApp.Business
{
	public class ViewModelsLocator
	{
		private static IContainer _container;

		public static void Initialize(IContainer container)
		{
			container.RegisterFactory(x => new MainPageViewModel(x));
			_container = container;
		}

		public MainPageViewModel MainPageViewModel
		{
			get { return _container.Resolve<MainPageViewModel>(); }
		}
	}
}
