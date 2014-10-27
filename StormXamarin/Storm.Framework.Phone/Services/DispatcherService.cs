using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Storm.Framework.Services
{
	public class DispatcherService : IDispatcherService
	{
		private Dispatcher m_dispatcher;

		public DispatcherService(Dispatcher dispatcher)
		{
			this.m_dispatcher = dispatcher;
		}

		public void InvokeOnUIThread(Action action)
		{
			this.m_dispatcher.BeginInvoke(action);
		}

		public void InvokeOnUIThread<T>(Func<T> action, Action<T> callback)
		{
			this.m_dispatcher.BeginInvoke(() =>
			{
				T result = action();
				callback(result);
			});
		}
	}
}
