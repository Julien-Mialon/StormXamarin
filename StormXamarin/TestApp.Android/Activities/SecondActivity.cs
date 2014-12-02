using Android.App;
using Android.OS;
using Storm.Mvvm;
using TestApp.Android.CompositionRoot;

namespace TestApp.Android.Activities
{
	[Activity(Label = "SecondActivity")]
	public partial class SecondActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Second);
			SetViewModel(Container.ViewModelsLocator.SecondPageViewModel);
		}
	}
}