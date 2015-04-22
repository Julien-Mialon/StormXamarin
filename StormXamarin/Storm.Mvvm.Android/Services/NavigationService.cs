using System;
using System.Collections.Generic;
using Android.Content;
using Java.Lang;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Navigation;
using Exception = System.Exception;

namespace Storm.Mvvm.Services
{
	public class NavigationService : AbstractNavigationService
	{
		private readonly Dictionary<string, Type> _views;

		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		public NavigationService(Dictionary<string, Type> views)
		{
			_views = views;
		}

		public override bool CanGoBack
		{
			get { return ActivityService.CurrentActivity != null; }
		}

		public override bool CanGoForward
		{
			get { return false; }
		}

		public override void GoBack()
		{
			ActivityService.CurrentActivity.OnBackPressed();
		}

		public override void GoForward()
		{
			throw new NotImplementedException();
		}

		public override void ExitApplication()
		{
			JavaSystem.RunFinalizersOnExit(true);
			JavaSystem.Exit(0);
		}

		protected override void RemoveBackEntry()
		{
			GoBack();
		}

		protected override void NavigateToView(string view, string parametersKey)
		{
			Type activityType = GetViewOrThrow(view);
			Intent activity = new Intent(ActivityService.CurrentActivity, activityType);
			activity.PutExtra("key", parametersKey);
			ActivityService.CurrentActivity.StartActivity(activity);
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
