using System.Threading.Tasks;

namespace Storm.Mvvm.Services
{
	public interface IDialogService
	{
		Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);

		Task DisplayAlertAsync(string title, string message, string cancel);

		Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] actions);
	}
}
