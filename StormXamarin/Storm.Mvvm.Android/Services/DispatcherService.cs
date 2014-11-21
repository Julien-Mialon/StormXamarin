using System;
using System.Threading;
using Android.App;
using Storm.Mvvm.Inject;

namespace Storm.Mvvm.Services
{
	public class DispatcherService : IDispatcherService, IActivityUpdatable
	{
		private Activity _currentActivity;

		public void UpdateActivity(Activity activity)
		{
			_currentActivity = activity;
		}

		public void InvokeOnUIThread(Action action)
		{
			if (SynchronizationContext.Current != null)
			{
				action();
			}
			else
			{
				_currentActivity.RunOnUiThread(action);
			}
		}

		public void InvokeOnUIThread<T>(Func<T> action, Action<T> callback)
		{
			Action execution = () =>
			{
				T result = action();
				callback(result);
			};
		
			if (SynchronizationContext.Current != null)
			{
				execution();
			}
			else
			{
				_currentActivity.RunOnUiThread(execution);
			}
		}
	}
}
