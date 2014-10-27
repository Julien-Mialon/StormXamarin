using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Framework.Services
{
	public interface IDispatcherService
	{
		void InvokeOnUIThread(Action action);

		void InvokeOnUIThread<T>(Func<T> action, Action<T> callback);
	}
}
