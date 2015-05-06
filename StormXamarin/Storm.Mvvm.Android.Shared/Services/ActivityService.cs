using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Storm.Mvvm.Events;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Services
{
	public class ActivityService : IActivityService
	{
		private Activity _currentActivity;

		#region Activity update methods

		public event EventHandler<ValueChangingEventArgs<Activity>> ActivityChanging;

		public event EventHandler<ValueChangedEventArgs<Activity>> ActivityChanged;

		public Activity CurrentActivity
		{
			get
			{
				return _currentActivity;
			}
			set
			{
				if (!Equals(_currentActivity, value))
				{
					Activity oldValue = _currentActivity;
					RaiseActivityChanging(oldValue, value);
					_currentActivity = value;
					RaiseActivityChanged(oldValue, value);
				}
			}
		}

		protected void RaiseActivityChanging(Activity oldValue, Activity newValue)
		{
			EventHandler<ValueChangingEventArgs<Activity>> handler = ActivityChanging;
			if (handler != null)
			{
				handler(this, new ValueChangingEventArgs<Activity>(oldValue, newValue));
			}
		}

		protected void RaiseActivityChanged(Activity oldValue, Activity newValue)
		{
			EventHandler<ValueChangedEventArgs<Activity>> handler = ActivityChanged;
			if (handler != null)
			{
				handler(this, new ValueChangedEventArgs<Activity>(oldValue, newValue));
			}
		}

		#endregion

		private readonly Dictionary<int, Action<Result, Intent>> _callbacks = new Dictionary<int, Action<Result, Intent>>();
		private int _requestCode = 1;
		private readonly object _asyncMutex = new object();

		public void StartActivityForResult(Intent intent, Action<Result, Intent> resultCallback)
		{
			if (_currentActivity == null)
			{
				throw new InvalidOperationException("Call StartActivityForResult before initializing any activity in the application");
			}

			lock (_asyncMutex)
			{
				_callbacks.Add(_requestCode, resultCallback);
				_currentActivity.StartActivityForResult(intent, _requestCode);
				_requestCode++;
			}
		}

		public void ProcessActivityResult(int requestCode, Result resultCode, Intent data)
		{
			Action<Result, Intent> resultCallback;
			lock (_asyncMutex)
			{
				if (_callbacks.ContainsKey(requestCode))
				{
					resultCallback = _callbacks[requestCode];
					_callbacks.Remove(requestCode);
				}
				else
				{
					throw new InvalidOperationException("Trying to get result for a non registered request");
				}
			}

			resultCallback(resultCode, data);
		}
	}
}
