using System;

using Android.Views;
using Storm.Mvvm;
using TestApp.Android.CompositionRoot;

namespace TestApp.Android.Activities
{
	public partial class TestFragment : FragmentBase
	{
		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			return inflater.Inflate(Resource.Layout.TestFragment, container, false);
		}

		protected override ViewModelBase CreateViewModel()
		{
			return Container.ViewModelsLocator.MainFragmentViewModel;
		}
	}
}