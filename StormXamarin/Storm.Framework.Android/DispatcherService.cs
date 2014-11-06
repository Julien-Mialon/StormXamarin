using System;
using Storm.Framework.Services;

namespace Storm.Framework.Android
{
	public class DispatcherService : IDispatcherService
	{
		public void InvokeOnUIThread(Action action)
		{
			throw new NotImplementedException();
		}

		public void InvokeOnUIThread<T>(Func<T> action, Action<T> callback)
		{
			throw new NotImplementedException();
		}
	}
}
