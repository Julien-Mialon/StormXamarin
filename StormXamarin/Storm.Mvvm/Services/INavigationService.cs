using System.Collections.Generic;

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
	}
}
