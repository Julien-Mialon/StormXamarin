using System.ComponentModel;
using Android.OS;
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
			View view = inflater.Inflate(Resource.Layout.MainFragment, container);

			SetViewModel(view, Container.ViewModelsLocator.MainFragmentViewModel);

			return view;
		}
	}
}
