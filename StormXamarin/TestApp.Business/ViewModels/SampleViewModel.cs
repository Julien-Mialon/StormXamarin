using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace TestApp.Business.ViewModels.Sample
{
	public class MainViewModel : ViewModelBase
	{
		private string _inputText;
		private string _labelText;

		// Property for the input text field 
		public string InputText
		{
			get { return _inputText; }
			set { SetProperty(ref _inputText, value); }
		}

		// Property for the text to display
		public string LabelText
		{
			get { return _labelText; }
			set { SetProperty(ref _labelText, value); }
		}

		// The command bound to the button click 
		public ICommand ButtonCommand { get; private set; }

		public MainViewModel()
		{
			// Associate the command with function 
			ButtonCommand = new DelegateCommand(ButtonAction);
		}

		private void ButtonAction()
		{
			// Update text of label
			LabelText = string.Format("Hello {0} !", InputText);
		}
	}
}
