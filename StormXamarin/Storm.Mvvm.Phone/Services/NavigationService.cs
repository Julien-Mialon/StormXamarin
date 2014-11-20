using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;

namespace Storm.Mvvm.Services
{
	public class NavigationService : INavigationService
	{
		#region Fields

		private readonly PhoneApplicationFrame _service = null;

		#endregion

		#region Properties

		public bool CanGoBack
		{
			get
			{
				return _service.CanGoBack;
			}
		}

		public bool CanGoForward
		{
			get
			{
				return _service.CanGoForward;
			}
		}

		#endregion

		#region Constructors

		public NavigationService(PhoneApplicationFrame service)
		{
			this._service = service;
		}

		#endregion

		#region Public methods

		public void Navigate(string view)
		{
			_service.Navigate(new Uri(string.Format("/Views/{0}.xaml", view), UriKind.Relative));
		}

		public void Navigate(string view, Dictionary<string, object> parameters)
		{
			_service.Navigate(new Uri(string.Format("/Views/{0}.xaml", view), UriKind.Relative));
		}

		public void NavigateAndReplace(string view)
		{
			Navigate(view);
			_service.RemoveBackEntry();
		}

		public void NavigateAndReplace(string view, Dictionary<string, object> parameters)
		{
			Navigate(view);
			_service.RemoveBackEntry();
		}

		public void GoBack()
		{
			_service.GoBack();
		}

		public void GoForward()
		{
			_service.GoForward();
		}

		#endregion
	}
}
