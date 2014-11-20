using System;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
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
