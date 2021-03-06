﻿using System;
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
				{Views.ADAPTER, typeof(AdapterActivity)},
			};
			Dictionary<string, Type> dialogs = new Dictionary<string, Type>
			{
				{Dialogs.MAIN, typeof(MainFragment)},
				{Dialogs.COLOR_PICKER, typeof(ColorPicker)},
			};

			AndroidContainer.CreateInstance<Container>(this, views, dialogs);
		}
	}
}
