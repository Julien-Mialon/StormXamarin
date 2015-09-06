using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Cirrious.MvvmCross.Droid.FullFragging.Fragments;

namespace Storm.MvvmCross.Android.Views
{
	public class StormDialogFragment : MvxDialogFragment
	{
		public event EventHandler Canceled;
		public event EventHandler Dismissed;

		public StormDialogFragment()
		{
			
		}
		//todo find event onviewmodel created

	}
}