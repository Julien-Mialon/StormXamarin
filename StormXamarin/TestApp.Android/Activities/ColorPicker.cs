using Storm.Mvvm;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Framework.ColorPicker;
using Storm.Mvvm.Inject;
using TestApp.Business.ViewModels;

namespace TestApp.Android.Activities
{
	[BindingElement(Path = "ActualColor", TargetPath = "OldColor")]
	[BindingElement(Path = "NewColor", TargetPath = "CurrentColor", Mode = BindingMode.TwoWay)]
	[BindingElement(Path = "OkCommand", TargetPath = "PositiveButtonEvent")]
	public class ColorPicker : AbstractColorPickerDialog
	{
		protected override ViewModelBase CreateViewModel()
		{
			return new ColorPickerViewModel(AndroidContainer.GetInstance());
		}
	}
}
