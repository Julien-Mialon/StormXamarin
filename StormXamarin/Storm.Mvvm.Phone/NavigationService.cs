using System;
using System.Collections.Generic;
using Microsoft.Phone.Controls;

namespace Storm.Mvvm
{
	public class NavigationService : INavigationService
	{
		#region Fields

		private PhoneApplicationFrame m_service = null;

		#endregion

		#region Properties

		public bool CanGoBack
		{
			get
			{
				return m_service.CanGoBack;
			}
		}

		public bool CanGoForward
		{
			get
			{
				return m_service.CanGoForward;
			}
		}

		#endregion

		#region Constructors

		public NavigationService(PhoneApplicationFrame _service)
		{
			m_service = _service;
		}

		#endregion

		#region Public methods

		public void Navigate(string _view)
		{
			m_service.Navigate(new Uri(string.Format("/Views/{0}.xaml", _view), UriKind.Relative));
		}

		public void Navigate(string _view, Dictionary<string, object> _parameters)
		{
			m_service.Navigate(new Uri(string.Format("/Views/{0}.xaml", _view), UriKind.Relative));
		}

		public void NavigateAndReplace(string _view)
		{
			Navigate(_view);
			m_service.RemoveBackEntry();
		}

		public void NavigateAndReplace(string _view, Dictionary<string, object> _parameters)
		{
			Navigate(_view);
			m_service.RemoveBackEntry();
		}

		public void GoBack()
		{
			m_service.GoBack();
		}

		public void GoForward()
		{
			m_service.GoForward();
		}

		#endregion
	}
}
