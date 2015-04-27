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

		public static Task<U> CreateAsyncFromCallback<T, U>(Action<Action<T>> asyncStarter, Func<T, U> resultHandler)
		{
			TaskCompletionSource<U> taskSource = new TaskCompletionSource<U>();
			Task<U> t = taskSource.Task;

			Task.Factory.StartNew(() =>
			{
				asyncStarter(res => taskSource.SetResult(resultHandler(res)));
			});

			return t;
		}
	}
}