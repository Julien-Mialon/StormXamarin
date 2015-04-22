using Android.App;
using Android.OS;
using Storm.Mvvm;
using TestApp.Android.CompositionRoot;

namespace TestApp.Android.Activities
{
	[Activity(Label="AdapterActivity")]
	public partial class AdapterActivity : ActivityBase
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.Adapter);
			SetViewModel(Container.ViewModelsLocator.AdapterViewModel);
		}
	}

}
