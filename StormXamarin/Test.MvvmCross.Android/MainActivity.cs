using Android.App;
using Android.OS;
using Storm.MvvmCross.Android.Views;
using Test.MvvmCross.Business;

namespace Test.MvvmCross.Android
{
	[Activity(Label = "Test.MvvmCross.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public partial class MainActivity : StormActivity<MainViewModel>
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.MainView);
		}
	}
}

