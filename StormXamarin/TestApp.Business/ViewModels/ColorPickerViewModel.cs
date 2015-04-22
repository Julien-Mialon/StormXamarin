using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace TestApp.Business.ViewModels
{
	public class ColorContainer : NotifierBase
	{
		private uint _color = 0xFFFF0000;

		public uint Color
		{
			get { return _color; }
			set { SetProperty(ref _color, value); }
		}
	}

	public class ColorPickerViewModel : ViewModelBase
	{
		public static ColorContainer ColorStatic = new ColorContainer();

		private uint _actualColor;
		private uint _newColor;

		public uint ActualColor
		{
			get { return _actualColor; }
			set { SetProperty(ref _actualColor, value); }
		}

		public uint NewColor
		{
			get { return _newColor; }
			set { SetProperty(ref _newColor, value); }
		}

		public ICommand OkCommand { get; private set; }

		public ColorPickerViewModel()
		{
			ActualColor = ColorStatic.Color;
			OkCommand = new DelegateCommand(OkAction);
		}

		private void OkAction()
		{
			ColorStatic.Color = _newColor;
		}
	}
}
