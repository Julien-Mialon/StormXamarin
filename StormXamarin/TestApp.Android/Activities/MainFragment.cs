using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Dialogs;
using Container = TestApp.Android.CompositionRoot.Container;

namespace TestApp.Android.Activities
{
	[BindingElement(Path = "PositiveCommand", TargetPath = "PositiveButtonEvent")]
	[BindingElement(Path = "NegativeCommand", TargetPath = "NegativeButtonEvent")]
	public partial class MainFragment : AlertDialogFragmentBase
	{
		public MainFragment()
		{
			Title = "My Title";
			Message = "My Message";
			Buttons.Add(DialogsButton.Positive, "Valider");
			Buttons.Add(DialogsButton.Negative, "Annuler");
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
