using System;
using System.Collections.Generic;
using Android.App;
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

		private readonly List<IActivityUpdatable> _activityUpdatables = new List<IActivityUpdatable>();

		protected Application Application { get; private set; }
		protected Activity CurrentActivity { get; private set; }

		protected NavigationService NavigationService;
		protected DispatcherService DispatcherService;

		protected AndroidContainer()
		{
			
		}

		public virtual void UpdateActivity(Activity currentActivity)
		{
			CurrentActivity = currentActivity;
			foreach (IActivityUpdatable updatable in _activityUpdatables)
			{
				updatable.UpdateActivity(CurrentActivity);
			}
		}

		protected virtual void Initialize(Application application, Dictionary<string, Type> views)
		{
			Application = application;

			//Create services
			NavigationService = new NavigationService(views);
			DispatcherService = new DispatcherService();
			
			//Register for activity update
			RegisterForActivityUpdate(NavigationService);
			RegisterForActivityUpdate(DispatcherService);

			//Register services
			RegisterInstance<INavigationService, NavigationService>(NavigationService);
			RegisterInstance<IDispatcherService, DispatcherService>(DispatcherService);

			Initialize();
		}

		protected void RegisterForActivityUpdate(IActivityUpdatable updatable)
		{
			if (CurrentActivity != null)
			{
				updatable.UpdateActivity(CurrentActivity);
			}
			_activityUpdatables.Add(updatable);
		}

		protected override void Clean()
		{
			
		}
	}
}
