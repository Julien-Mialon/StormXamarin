using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Util;
using Android.Widget;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using TestApp.Android.Converters;
using TestApp.Android.Selectors;

namespace TestApp.Android.Activities
{
	[Activity(Label = "TestApp.Android", MainLauncher = true, Icon = "@drawable/icon")]
	public partial class MainActivity : ActivityBase
	{
		private string _myString;

		private Random _generator = new Random((int)DateTime.Now.Ticks);

		[Binding("InputText", Converter = typeof(StringToUpperConverter))]
		public string MyString
		{
			get { return _myString; }
			set
			{
				_myString = value;
				Toast.MakeText(this, "I just changed !", ToastLength.Short);
			}
		}

		//[Binding("ColorStatic.Color")]
		//public uint CurrentColor
		//{
		//	get { return 0; }
		//	set
		//	{
		//		Log.Wtf("SetColor", "ColorValue = " + value);
		//		ColorPanel.Background = new ColorDrawable(new Color((int) value));
		//	}
		//}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);
			SetViewModel(TestApp.Android.CompositionRoot.Container.ViewModelsLocator.MainPageViewModel);

			//Spinner spinner = FindViewById<Spinner>(Resource.Id.ComboBox);

			//List<string> spinnerList = new List<string>()
			//{
			//	"NY", "Paris", "London", "Berlin", "Moscov", "Gdansk"
			//};
			//BindableArrayAdapter<string> adapter = new BindableArrayAdapter<string>(
			//	spinnerList, this, 
			//	new SpinnerViewSelector(this.LayoutInflater, global::Android.Resource.Layout.SimpleSpinnerItem));
			//ArrayAdapter<string> javaAdapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, spinnerList);
			//spinner.Adapter = adapter;
			
			//spinner.SelectedItem = null;
			//spinner.SelectedItemPosition = 4;
			//spinner.SelectedItemId = 12;
			//spinner.SelectedView = null;
		}

	}
}

