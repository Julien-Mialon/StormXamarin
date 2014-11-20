using System;
using System.Windows.Threading;

namespace Storm.Mvvm.Services
{
	public class DispatcherService : IDispatcherService
	{
		private readonly Dispatcher _dispatcher;

		public DispatcherService(Dispatcher dispatcher)
		{
			_dispatcher = dispatcher;
		}

		public void InvokeOnUIThread(Action action)
		{
			_dispatcher.BeginInvoke(action);
		}

		public void InvokeOnUIThread<T>(Func<T> action, Action<T> callback)
		{
			_dispatcher.BeginInvoke(() =>
			{
				T result = action();
				callback(result);
			});
		}
	}
}
