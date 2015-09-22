using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Storm.MvvmCross;

namespace Test.MvvmCross.Business
{
	public class MainViewModel : StormViewModel
	{
		private string _message;
		private string _input;

		public string Message
		{
			get { return _message; }
			set { SetProperty(ref _message, value); }
		}

		public string Input
		{
			get { return _input; }
			set { SetProperty(ref _input, value); }
		}

		public ICommand OkCommand { get; private set; }

		public MainViewModel()
		{
			OkCommand = new MvxCommand(OkAction);
		}

		private void OkAction()
		{
			Message = string.Format("Hello {0} !", string.IsNullOrWhiteSpace(Input) ? "Anonymous" : Input);
		}
	}
}
