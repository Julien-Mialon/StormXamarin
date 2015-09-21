using System;
using System.Threading.Tasks;

namespace TestApp.Android.Service
{
	public static class AsyncHelper
	{
		public static Task<T> CreateAsyncFromCallback<T>(Action<Action<T>> asyncStarter)
		{
			TaskCompletionSource<T> taskSource = new TaskCompletionSource<T>();
			Task<T> t = taskSource.Task;

			Task.Factory.StartNew(() =>
			{
				asyncStarter(taskSource.SetResult);
			});

			return t;
		}

		public static Task<TU> CreateAsyncFromCallback<T, TU>(Action<Action<T>> asyncStarter, Func<T, TU> resultHandler)
		{
			TaskCompletionSource<TU> taskSource = new TaskCompletionSource<TU>();
			Task<TU> t = taskSource.Task;

			Task.Factory.StartNew(() =>
			{
				asyncStarter(res => taskSource.SetResult(resultHandler(res)));
			});

			return t;
		}
	}
}