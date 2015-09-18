using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Storm.Mvvm;
using Storm.Mvvm.Adapters;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.TemplateSelectors;
using TestApp.Android.Converters;

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
			SetViewModel(CompositionRoot.Container.ViewModelsLocator.MainPageViewModel);

			Spinner spinner = LazyComboBox;
			//Spinner spinner = new Spinner(BaseContext);
			spinner.Adapter = new LazyBindableAdapter(BaseContext, new EnumerableCursor(Enumerable.Range(0, 10)))
			{
				TemplateSelector = new IntTemplateSelector(BaseContext)
			};

			//Spinner spinner = FindViewById<Spinner>(Resource.Id.ComboBox);

			//List<string> spinnerList = new List<string>()
			//{
			//	"NY", "Paris", "London", "Berlin", "Moscov", "Gdansk"
			//};
			//BindableAdapter<string> adapter = new BindableAdapter<string>(
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

	public class IntTemplateSelector : AbstractTemplateSelector
	{
		private Context _context;

		public IntTemplateSelector(Context ctx)
		{
			_context = ctx;
		}

		public override View GetView(object model, ViewGroup parent, View oldView)
		{
			TextView resultView = null;

			if (oldView != null)
			{
				resultView = oldView as TextView;
			}
			if(resultView == null)
			{
				resultView = new TextView(_context);
				resultView.SetTextColor(new Color(0, 255, 0));
			}

			resultView.Text = model.ToString();

			return resultView;
		}

		public override DataTemplate GetTemplate(object model)
		{
			throw new NotImplementedException();
		}
	}
}

