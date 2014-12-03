using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace TestApp.Business.ViewModels
{
	public class DataContainer : NotifierBase
	{
		private string _text;
		private int _count;

		public string Text
		{
			get { return _text; }
			set { SetProperty(ref _text, value); }
		}

		public int Count
		{
			get { return _count; }
			set { SetProperty(ref _count, value); }
		}
	}

    public class MainPageViewModel : ViewModelBase
    {
		private string _buttonText = "";
		private int _counter = 0;
	    private string _inputText;
	    private DataContainer _data;

	    public string InputText
	    {
			get { return _inputText; }
			set
			{
				SetProperty(ref _inputText, value);
			}
	    }

		public string ButtonText
		{
			get { return _buttonText; }
			set { SetProperty(ref _buttonText, value); }
		}

	    public DataContainer Data
	    {
			get { return _data; }
			private set { SetProperty(ref _data, value); }
	    }

		public ICommand ButtonCommand { get; private set; }

		public MainPageViewModel(IContainer container)
			: base(container)
		{
			Data = new DataContainer();
			ButtonCommand = new DelegateCommand(ButtonAction);
			ButtonText = "Hello world !!!!";

			ILocalizationService localizationService = container.Resolve<ILocalizationService>();
			string name = localizationService.GetString("Hello");
			ButtonText = name;
		}

		private void ButtonAction()
		{
			_counter++;

			Data.Count = _counter;
			Data.Text = "Plop x " + _counter;

			ButtonText = string.Format("You've clicked {0} times on this button", _counter);

			NavigationService.Navigate(Views.SECOND, new Dictionary<string, object>(){{"Greetings", "Plop !"}});
		}
    }
}
