using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Inject
{
	public class PhoneContainer : ContainerBase
	{
		#region Static members

		private static PhoneContainer _instance;

		public static T CreateInstance<T>(PhoneApplicationFrame rootFrame, Dictionary<string, string> views, object stringResources) where T : PhoneContainer, new()
		{
			if (_instance != null)
			{
				throw new Exception("Can not create two instance of the container !");
			}
			T container = new T();
			_instance = container;

			container.Initialize(rootFrame, views ?? new Dictionary<string, string>(), stringResources);
			return container;
		}

		public static PhoneContainer GetInstance()
		{
			if (_instance == null)
			{
				throw new Exception("PhoneContainer has not been created");
			}
			return _instance;
		}

		#endregion

		private PhoneApplicationFrame _rootFrame;

		protected virtual void Initialize(PhoneApplicationFrame rootFrame, Dictionary<string, string> views, object stringResources)
		{
			_rootFrame = rootFrame;

			if (stringResources != null)
			{
				RegisterInstance<ILocalizationService>(new LocalizationService(stringResources));
			}
			RegisterInstance<INavigationService>(new NavigationService(_rootFrame, views));
			RegisterInstance<IDispatcherService>(new DispatcherService(_rootFrame.Dispatcher));
			RegisterInstance<ILoggerService>(new LoggerService());

			Initialize();
		}
	}
}
