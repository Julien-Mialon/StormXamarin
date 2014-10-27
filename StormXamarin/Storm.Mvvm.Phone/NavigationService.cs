using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			this.m_service = _service;
		}

		#endregion

		#region Public methods

		public void Navigate(string _view)
		{
			this.m_service.Navigate(new Uri(string.Format("/Views/{0}.xaml", _view), UriKind.Relative));
		}

		public void Navigate(string _view, Dictionary<string, object> _parameters)
		{
			this.m_service.Navigate(new Uri(string.Format("/Views/{0}.xaml", _view), UriKind.Relative));
		}

		public void NavigateAndReplace(string _view)
		{
			this.Navigate(_view);
			this.m_service.RemoveBackEntry();
		}

		public void NavigateAndReplace(string _view, Dictionary<string, object> _parameters)
		{
			this.Navigate(_view);
			this.m_service.RemoveBackEntry();
		}

		public void GoBack()
		{
			this.m_service.GoBack();
		}

		public void GoForward()
		{
			this.m_service.GoForward();
		}

		#endregion
	}
}
