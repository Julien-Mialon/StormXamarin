using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Storm.Mvvm.Services
{
	public class DispatcherService : IDispatcherService
	{
		private readonly Frame _frame;

		public DispatcherService(Frame frame)
		{
			_frame = frame;
		}

		public void InvokeOnUIThread(Action action)
		{
			_frame.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
		}

		public void InvokeOnUIThread<T>(Func<T> action, Action<T> callback)
		{
			_frame.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
			{
				T result = action();
				Task.Run(() => callback(result));
			});
		}
	}
}
