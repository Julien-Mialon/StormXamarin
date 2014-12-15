using System.Collections.Generic;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Navigation
{
	public abstract class AbstractNavigationService : INavigationService
	{
		private readonly Dictionary<string, NavigationParametersContainer> _parametersContainers = new Dictionary<string, NavigationParametersContainer>();

		public abstract bool CanGoBack { get; }
		public abstract bool CanGoForward { get; }

		public abstract void GoBack();
		public abstract void GoForward();

		public abstract void ExitApplication();

		public virtual void Navigate(string view)
		{
			NavigateToView(view, null);
		}

		public virtual void Navigate(string view, Dictionary<string, object> parameters)
		{
			string key = CreateContainer(view, parameters);
			NavigateToView(view, key);
		}

		public virtual void NavigateAndReplace(string view)
		{
			RemoveBackEntry();
			Navigate(view);
		}

		public virtual void NavigateAndReplace(string view, Dictionary<string, object> parameters)
		{
			RemoveBackEntry();
			Navigate(view, parameters);
		}

		public virtual NavigationParametersContainer GetParameters(string parametersKey)
		{
			if (parametersKey != null && _parametersContainers.ContainsKey(parametersKey))
			{
				return _parametersContainers[parametersKey];
			}
			return null;
		}

		public string StoreMessageDialogParameters(string dialog, Dictionary<string, object> parameters)
		{
			return CreateContainer(string.Format("_DIALOG_{0}", dialog), parameters);
		}

		protected virtual string CreateContainer(string view, Dictionary<string, object> parameters)
		{
			NavigationParametersContainer container = new NavigationParametersContainer(view, parameters);
			_parametersContainers.Add(container.Key, container);

			return container.Key;
		}
		
		protected abstract void RemoveBackEntry();
		protected abstract void NavigateToView(string view, string parametersKey);
	}
}
