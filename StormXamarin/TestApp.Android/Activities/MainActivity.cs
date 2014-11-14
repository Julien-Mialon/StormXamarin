using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Storm.Mvvm.Android;
using Storm.Mvvm.Android.Bindings;
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

		//protected override List<BindingObject> GetBindingPaths()
		//{
		//	List<BindingObject> binders = new List<BindingObject>();

		//	BindingObject myButton = new BindingObject()
		//	{
		//		TargetObjectName = "MyButton",
		//	};
		//	List<BindingExpression> expressions = new List<BindingExpression>
		//	{
		//		new BindingExpression()
		//		{
		//			SourcePath = "ButtonText",
		//			TargetField = "Text",
		//		},
		//		new BindingExpression()
		//		{
		//			SourcePath = "ButtonCommand",
		//			TargetField = "Click",
		//		}
		//	};

		//	myButton.Expressions = expressions;
		//	binders.Add(myButton);
		//	return binders;
		//}
	}
}

