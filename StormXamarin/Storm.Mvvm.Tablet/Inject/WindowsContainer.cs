using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Inject
{
	public class WindowsContainer : ContainerBase
	{
		#region Static members

		private static WindowsContainer _instance = null;

		public static T CreateInstance<T>(Frame rootFrame, Dictionary<string, Type> views, Dictionary<string, Type> dialogs = null) where T : WindowsContainer, new()
		{
			if (_instance != null)
			{
				throw new Exception("Can not create two instance of the container !");
			}
			T container = new T();
			_instance = container;

			container.Initialize(rootFrame, views ?? new Dictionary<string, Type>(), dialogs ?? new Dictionary<string, Type>());
			return container;
		}

		public static WindowsContainer GetInstance()
		{
			if (_instance == null)
			{
				throw new Exception("WindowsContainer has not been created");
			}
			return _instance;
		}

		#endregion

		private Frame _rootFrame;

		public void Initialize(Frame rootFrame, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			_rootFrame = rootFrame;


			RegisterInstance<ILocalizationService>(new LocalizationService());
			RegisterInstance<INavigationService>(new NavigationService(_rootFrame, views));
			RegisterInstance<IMessageDialogService>(new MessageDialogService(dialogs));
			RegisterInstance<IDispatcherService>(new DispatcherService(_rootFrame));
			RegisterInstance<ILoggerService>(new LoggerService());
		}
	}
}
