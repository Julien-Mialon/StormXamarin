using System;
using System.Collections.Generic;
using Android.App;

namespace Storm.Mvvm.Services
{
	public class NavigationService : INavigationService
	{
		private Activity _currentActivity;
		private readonly Dictionary<string, Type> _views; 

		public NavigationService(Dictionary<string, Type> views)
		{
			_views = views;
		}

		public void UpdateActivity(Activity currentActivity)
		{
			_currentActivity = currentActivity;
		}

		public bool CanGoBack
		{
			get { return true; }
		}

		public bool CanGoForward
		{
			get { return false; }
		}

		public void Navigate(string view)
		{
			Type viewType = GetViewOrThrow(view);
			_currentActivity.StartActivity(viewType);
		}

		public void Navigate(string view, Dictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}

		public void NavigateAndReplace(string view)
		{
			throw new NotImplementedException();
		}

		public void NavigateAndReplace(string view, Dictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}

		public void GoBack()
		{
			throw new NotImplementedException();
		}

		public void GoForward()
		{
			throw new NotImplementedException();
		}

		protected Type GetViewOrThrow(string view)
		{
			if (_views.ContainsKey(view))
			{
				return _views[view];
			}
			throw new Exception(string.Format("View {0} has not been registered in the NavigationService", view));
		}
	}
}
