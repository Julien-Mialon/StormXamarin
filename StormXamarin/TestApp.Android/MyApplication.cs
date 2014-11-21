using System;
using System.Collections.Generic;
using Android.App;
using Android.Runtime;
using Storm.Mvvm;
using Storm.Mvvm.Inject;
using TestApp.Android.Activities;
using TestApp.Android.CompositionRoot;
using TestApp.Business;

namespace TestApp.Android
{
	[Application]
	public class MyApplication : ApplicationBase
	{
		public MyApplication(IntPtr handle, JniHandleOwnership transfer)
            : base(handle,transfer)
        {

        }

		public override void OnCreate()
		{
			base.OnCreate();

			Dictionary<string, Type> views = new Dictionary<string, Type>
			{
				{Views.MAIN, typeof(MainActivity)},
				{Views.SECOND, typeof(SecondActivity)},
			};

			AndroidContainer.CreateInstance<Container>(this, views);
		}
	}
}
