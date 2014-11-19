using Storm.Framework.Android;
using Storm.Framework.Services;
using Storm.Mvvm;
using Storm.Mvvm.Android;
using TestApp.Business;

namespace TestApp.Android.CompositionRoot
{
	class BootStrapper
	{
		public static ViewModelsLocator ViewModelsLocator = new ViewModelsLocator();

		private static IContainer _container = null;
		private static INavigationService _navigationService = null;
		private static IDispatcherService _dispatcherService = null;

		private static bool _initialized = false;
		private static readonly object _mutex = new object();

		public static void Initialize()
		{
			lock (_mutex)
			{
				if (_initialized)
				{
					return;
				}

				_container = new ContainerBase();
				ViewModelsLocator.Initialize(_container);

				InitializeService();
				RegisterService();

				_initialized = true;
			}
		}

		private static void InitializeService()
		{
			_navigationService = new NavigationService();
			_dispatcherService = new DispatcherService();
		}

		private static void RegisterService()
		{
			_container.RegisterInstance<INavigationService>(_navigationService);
			_container.RegisterInstance<IDispatcherService>(_dispatcherService);
		}
	}
}
