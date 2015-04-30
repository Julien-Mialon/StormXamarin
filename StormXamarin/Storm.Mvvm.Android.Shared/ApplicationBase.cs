using System;
using Android.App;
using Android.Runtime;

namespace Storm.Mvvm
{
	public class ApplicationBase : Application
	{
		public ApplicationBase(IntPtr handle, JniHandleOwnership transfer) : base(handle,transfer)
		{

		}
	}
}
