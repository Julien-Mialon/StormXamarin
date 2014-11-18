using Android.App;
using Android.OS;
using Storm.Mvvm.Android;
using TestApp.Android.CompositionRoot;

namespace TestApp.Android.Activities
{
	[Activity(Label = "TestApp.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public partial class MainActivity : ActivityBase
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			BootStrapper.Initialize();

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			SetViewModel(BootStrapper.ViewModelsLocator.MainPageViewModel, typeof(Resource.Id));
		}
	}
}

