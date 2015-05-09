using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace TestApp.Business.ViewModels
{
	public class NavigationViewModel : ViewModelBase
	{
		private string _name;

		[NavigationParameter]
		public string Name
		{
			get { return _name; }
			set { SetProperty<string>(ref _name, value); }
		}

		public ICommand NavigationBackCommand { get; private set; }

		public ICommand DialogCloseCommand { get; private set; }

		public ICommand OpenDialogCommand { get; private set; }

		public NavigationViewModel()
		{
			NavigationBackCommand = new DelegateCommand(NavigationBackAction);
			DialogCloseCommand = new DelegateCommand(DialogCloseAction);
			OpenDialogCommand = new DelegateCommand(OpenDialogAction);
		}

		private void OpenDialogAction()
		{
			LazyResolver<IMessageDialogService>.Service.Show(Dialogs.MAIN);
		}

		public override void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			base.OnNavigatedTo(e, parametersKey);
		}

		private void DialogCloseAction()
		{
			LazyResolver<IMessageDialogService>.Service.DismissCurrentDialog();
		}

		private void NavigationBackAction()
		{
			LazyResolver<INavigationService>.Service.GoBack();
		}
	}
}
