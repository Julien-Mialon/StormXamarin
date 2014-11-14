using System.Windows.Input;
using Storm.Mvvm;

namespace TestApp.Business
{
    public class MainPageViewModel : ViewModelBase
    {
		private string _buttonText = "";
		private int _counter = 0;

		public string ButtonText
		{
			get { return _buttonText; }
			set { SetProperty(ref _buttonText, value); }
		}

		public ICommand ButtonCommand { get; private set; }

		public MainPageViewModel(IContainer container)
			: base(container)
		{
			ButtonCommand = new DelegateCommand(ButtonAction);
			ButtonText = "Hello world !!!!";
		}

		private void ButtonAction()
		{
			_counter++;

			ButtonText = string.Format("You've clicked {0} times on this button", _counter);
		}
    }
}
