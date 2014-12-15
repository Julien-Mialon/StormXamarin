using System.Collections.Generic;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public interface INavigationService
	{
		bool CanGoBack { get; }

		bool CanGoForward { get; }

		void Navigate(string view);

		void Navigate(string view, Dictionary<string, object> parameters);

		void NavigateAndReplace(string view);

		void NavigateAndReplace(string view, Dictionary<string, object> parameters);

		void GoBack();

		void GoForward();

		NavigationParametersContainer GetParameters(string parametersKey);

		string StoreMessageDialogParameters(string dialog, Dictionary<string, object> parameters);

		void ExitApplication();
	}
}
