using System;
using System.ComponentModel;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Storm.Mvvm;
using TestApp.Business;
using Container = TestApp.Android.CompositionRoot.Container;

namespace TestApp.Android.Activities
{
	public partial class MainFragment : FragmentBase
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			return base.OnCreateView(inflater, container, savedInstanceState);
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
