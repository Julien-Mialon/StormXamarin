using Storm.Mvvm;
using Storm.Mvvm.Inject;

namespace TestApp.Business
{
	public class SecondPageViewModel : ViewModelBase
	{
		private string _greetings;

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
