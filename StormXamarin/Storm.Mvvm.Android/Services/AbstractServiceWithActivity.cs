using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Services
{
	public abstract class AbstractServiceWithActivity
	{
		protected IActivityService ActivityService;

		protected Activity CurrentActivity
		{
			get { return ActivityService.CurrentActivity; }
		}
	}
}
