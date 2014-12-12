using System;
using Android.App;
using Storm.Mvvm.Events;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Services
{
	public class ActivityService : IActivityService
	{
		private Activity _currentActivity;

		public event EventHandler<ValueChangingEventArgs<Activity>> ActivityChanging;

		public event EventHandler<ValueChangedEventArgs<Activity>> ActivityChanged;

		public Activity CurrentActivity
		{
			get
			{
				if (_currentActivity == null)
				{
					throw new Exception("Activity == null");
				}
				return _currentActivity;
			}
			set
			{
				if (!object.Equals(_currentActivity, value))
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
	}
}
