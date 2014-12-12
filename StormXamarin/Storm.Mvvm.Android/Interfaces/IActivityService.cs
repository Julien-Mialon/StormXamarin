using System;
using Android.App;
using Storm.Mvvm.Events;

namespace Storm.Mvvm.Interfaces
{
	public interface IActivityService
	{
		event EventHandler<ValueChangingEventArgs<Activity>> ActivityChanging;

		event EventHandler<ValueChangedEventArgs<Activity>> ActivityChanged;

		Activity CurrentActivity { get; set; }
	}
}