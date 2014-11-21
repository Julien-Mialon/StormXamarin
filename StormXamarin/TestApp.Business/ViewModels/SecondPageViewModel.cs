using Storm.Mvvm;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;

namespace TestApp.Business.ViewModels
{
	public class SecondPageViewModel : ViewModelBase
	{
		private string _greetings;

		[NavigationParameter]
		public string Greetings
		{
			get { return _greetings; }
			set { SetProperty(ref _greetings, value); }
		}

		public SecondPageViewModel(IContainer container) : base(container)
		{
			Greetings = "Bonjour !";
		}
	}
}
