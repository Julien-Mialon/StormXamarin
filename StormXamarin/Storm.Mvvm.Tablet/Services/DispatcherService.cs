using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm.Services
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
