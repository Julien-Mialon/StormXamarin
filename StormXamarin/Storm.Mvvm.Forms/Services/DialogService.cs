using System.Threading.Tasks;
using Storm.Mvvm.Patterns;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public class DialogService : IDialogService
	{
		protected Page CurrentPage => LazySingletonInitializer<ICurrentPageService>.Value.CurrentPage;

		public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel)
		{
			return CurrentPage.DisplayAlert(title, message, accept, cancel);
		}

		public Task DisplayAlertAsync(string title, string message, string cancel)
		{
			return CurrentPage.DisplayAlert(title, message, cancel);
		}

		public Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] actions)
		{
			return CurrentPage.DisplayActionSheet(title, cancel, destruction, actions);
		}
	}
}
