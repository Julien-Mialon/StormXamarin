using System;
using System.Reflection;
using Android.App;
using Android.Runtime;

namespace Storm.Mvvm
{
	public class ApplicationBase : Application
	{
		public static Assembly MainAssembly { get; set; }

		public ApplicationBase(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer)
        {
			MainAssembly = Assembly.GetCallingAssembly();
        }

		public ApplicationBase(IntPtr handle, JniHandleOwnership transfer, Assembly entryAssembly) : base(handle, transfer)
		{
			MainAssembly = entryAssembly;
		}
	}
}
