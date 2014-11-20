using System;
using System.Collections.Generic;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public class NavigationService : INavigationService
	{
		public bool CanGoBack
		{
			get { return true; }
		}

		public bool CanGoForward
		{
			get { return false; }
		}

		public void Navigate(string view)
		{
			throw new NotImplementedException();
		}

		public void Navigate(string view, Dictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}

		public void NavigateAndReplace(string view)
		{
			throw new NotImplementedException();
		}

		public void NavigateAndReplace(string view, Dictionary<string, object> parameters)
		{
			throw new NotImplementedException();
		}

		public void GoBack()
		{
			throw new NotImplementedException();
		}

		public void GoForward()
		{
			throw new NotImplementedException();
		}
	}
}
