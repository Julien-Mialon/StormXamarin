using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Storm.Mvvm.Android
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

		public void Navigate(string _view)
		{
			throw new NotImplementedException();
		}

		public void Navigate(string _view, Dictionary<string, object> _parameters)
		{
			throw new NotImplementedException();
		}

		public void NavigateAndReplace(string _view)
		{
			throw new NotImplementedException();
		}

		public void NavigateAndReplace(string _view, Dictionary<string, object> _parameters)
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
