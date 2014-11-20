using System;

namespace Storm.Mvvm.Services
{
	public interface IDispatcherService
	{
		// ReSharper disable once InconsistentNaming
		void InvokeOnUIThread(Action action);

		// ReSharper disable once InconsistentNaming
		void InvokeOnUIThread<T>(Func<T> action, Action<T> callback);
	}
}
