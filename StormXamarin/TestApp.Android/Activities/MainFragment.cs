using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Dialogs;
using TestApp.Business;
using Container = TestApp.Android.CompositionRoot.Container;

namespace TestApp.Android.Activities
{
	public partial class MainFragment : DialogFragmentBase
	{
		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			return CreateDialog(savedInstanceState, "My Title", "My message", new Dictionary<DialogsButton, string>()
			{
				{DialogsButton.Positive, "Valider"},
				{DialogsButton.Negative, "Annuler"}
			});
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.MainFragment, container, false);
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.ViewModelsLocator.MainFragmentViewModel;
		}
	}
}
