using System;
using System.Collections.Generic;
using Android.App;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Inject
{
	public class AndroidContainer : ContainerBase
	{
		#region Static members

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
			if (_instance == null)
			{
				throw new Exception("AndroidContainer has not been created");
			}
			return _instance;
		}

		#endregion

		protected Application Application { get; private set; }

		protected IActivityService ActivityService;
		protected NavigationService NavigationService;
		protected DispatcherService DispatcherService;

		protected AndroidContainer()
		{
			ActivityService = new ActivityService();
		}

		protected AndroidContainer(IActivityService activityService)
		{
			if (activityService == null)
			{
				throw new ArgumentException("activityService");
			}
			ActivityService = activityService;
		}

		public virtual void UpdateActivity(Activity currentActivity)
		{
			ActivityService.CurrentActivity = currentActivity;
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

		protected override void Clean()
		{
			
		}
	}
}
