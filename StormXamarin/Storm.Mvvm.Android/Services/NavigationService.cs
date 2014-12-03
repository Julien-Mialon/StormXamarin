using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class NavigationService : AbstractNavigationService, IActivityUpdatable
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

		public override bool CanGoBack
		{
			get { return _currentActivity != null; }
		}

		public override bool CanGoForward
		{
			get { return false; }
		}

		public override void GoBack()
		{
			_currentActivity.OnBackPressed();
		}

		public override void GoForward()
		{
			throw new NotImplementedException();
		}

		public override void ExitApplication()
		{
			_currentActivity.Finish();
		}

		protected override void RemoveBackEntry()
		{
			GoBack();
		}

		protected override void NavigateToView(string view, string parametersKey)
		{
			Type activityType = GetViewOrThrow(view);
			Intent activity = new Intent(_currentActivity, activityType);
			activity.PutExtra("key", parametersKey);
			_currentActivity.StartActivity(activity);
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
