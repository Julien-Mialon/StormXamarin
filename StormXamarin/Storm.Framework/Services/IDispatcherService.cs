using System;

namespace Storm.Framework.Services
{
	public interface IDispatcherService
	{
		void InvokeOnUIThread(Action action);

		void InvokeOnUIThread<T>(Func<T> action, Action<T> callback);
	}
}
