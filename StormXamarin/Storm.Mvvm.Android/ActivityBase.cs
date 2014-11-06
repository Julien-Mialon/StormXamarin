using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Android.OS;

namespace Storm.Mvvm.Android
{
	public class ActivityBase : Activity
	{
		protected ViewModelBase ViewModel { get; private set; }

		protected void SetViewModel(ViewModelBase viewModel)
		{
			ViewModel = viewModel;

			InitializeComponents();
		}

		protected virtual void InitializeComponents()
		{
			
		}
	}
}
