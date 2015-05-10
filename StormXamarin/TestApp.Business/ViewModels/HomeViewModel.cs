using System.Collections.Generic;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace TestApp.Business.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
		private string _greetings;
		private string _name;

		public string Name
		{
			get { return _name; }
			set
			{
				if (SetProperty<string>(ref _name, value))
				{
					Greetings = string.Format("I'm {0}", value);
				}
			}
		}

		public string Greetings
		{
			get { return _greetings; }
			set { SetProperty<string>(ref _greetings, value); }
		}

		public ICommand NavigationCommand { get; private set; }

		public ICommand DialogCommand { get; private set; }

		public HomeViewModel()
		{
			NavigationCommand = new DelegateCommand(NavigationAction);
			DialogCommand = new DelegateCommand(DialogAction);
		}

		private void DialogAction()
		{
			LazyResolver<IMessageDialogService>.Service.Show(Dialogs.NAVIGATION_DIALOG, new Dictionary<string, object>
			{
				{"Name", Name}
			});
		}

		private void NavigationAction()
		{
			NavigationService.Navigate(Views.NAVIGATION_PAGE, new Dictionary<string, object>
			{
				{"Name", Name}
			});
		}
	}
}
