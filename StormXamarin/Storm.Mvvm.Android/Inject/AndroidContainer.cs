using System;
using System.Collections.Generic;
using Android.App;
using Java.Security;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Inject
{
	public class AndroidContainer : ContainerBase
	{
		private static AndroidContainer _instance = null;
		public static T CreateInstance<T>(Application application, Dictionary<string, Type> views) where T : AndroidContainer, new()
		{
			if (_instance != null)
			{
				throw new Exception("Can not create two instance of the container !");
			}
			T container = new T();
			_instance = container;

			container.Initialize(application, views);
			return container;
		}

		public static AndroidContainer GetInstance()
		{
			return _instance;
		}

		protected Application Application { get; private set; }

		protected NavigationService NavigationService;
		protected DispatcherService DispatcherService;

		protected AndroidContainer()
		{
			
		}

		public virtual void UpdateActivity(Activity currentActivity)
		{
			NavigationService.UpdateActivity(currentActivity);
		}

		protected virtual void Initialize(Application application, Dictionary<string, Type> views)
		{
			Application = application;

			//Create services
			NavigationService = new NavigationService(views);
			DispatcherService = new DispatcherService();
			
			//Register services
			RegisterInstance<INavigationService, NavigationService>(NavigationService);
			RegisterInstance<IDispatcherService, DispatcherService>(DispatcherService);

			Initialize();
		}

		protected virtual void Clean()
		{
			
		}
	}
}
