using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace TestApp.Business.ViewModels
{
	public class MainFragmentViewModel : ViewModelBase
	{
		private string _inputText;
		private string _labelText;

		public string InputText
		{
			get { return _inputText; }
			set { SetProperty(ref _inputText, value); }
		}

		public string LabelText
		{
			get { return _labelText; }
			set { SetProperty(ref _labelText, value); }
		}

		public ICommand ButtonCommand { get; private set; }

		public ICommand PositiveCommand { get; private set; }

		public ICommand NegativeCommand { get; private set; }

		public MainFragmentViewModel(IContainer container) : base(container)
		{
			InputText = "";
			LabelText = "";
			ButtonCommand = new DelegateCommand(ButtonAction);

			PositiveCommand = new DelegateCommand(PositiveAction);
			NegativeCommand = new DelegateCommand(NegativeAction);
		}

		private void NegativeAction()
		{
			LoggerService.Log("===> NegativeAction", MessageSeverity.Error);
		}

		private void PositiveAction()
		{
			LoggerService.Log("===> PositiveAction", MessageSeverity.Error);
		}

		private void ButtonAction()
		{
			LabelText = string.Format("Hello {0} !", InputText);
			InputText = "";
		}
	}
}
