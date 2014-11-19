using System;
using System.Windows.Threading;

namespace Storm.Framework.Services
{
	public class DispatcherService : IDispatcherService
	{
		private Dispatcher m_dispatcher;

		public DispatcherService(Dispatcher dispatcher)
		{
			m_dispatcher = dispatcher;
		}

		public void InvokeOnUIThread(Action action)
		{
			m_dispatcher.BeginInvoke(action);
		}

		public void InvokeOnUIThread<T>(Func<T> action, Action<T> callback)
		{
			m_dispatcher.BeginInvoke(() =>
			{
				T result = action();
				callback(result);
			});
		}
	}
}
