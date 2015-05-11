using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Phone.Controls;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class NavigationService : AbstractNavigationService
	{
		#region Fields

		private readonly PhoneApplicationFrame _service;
		private readonly Dictionary<string, string> _views; 

		#endregion

		#region Properties

		public override bool CanGoBack
		{
			get
			{
				return _service.CanGoBack;
			}
		}

		public override bool CanGoForward
		{
			get
			{
				return _service.CanGoForward;
			}
		}

		#endregion

		#region Constructors

		public NavigationService(PhoneApplicationFrame service, Dictionary<string, string> views)
		{
			_service = service;
			_views = views;
		}

		#endregion

		#region Public methods

		protected override void RemoveBackEntry()
		{
			_service.RemoveBackEntry();
		}

		protected override void NavigateToView(string view, string parametersKey)
		{
			string viewUri = GetViewOrThrow(view);
			_service.Navigate(new Uri(string.Format("{0}?key={1}", viewUri, parametersKey), UriKind.Relative));
		}

		protected virtual string GetViewOrThrow(string view)
		{
			if (_views.ContainsKey(view))
			{
				return _views[view];
			}
			throw new Exception(string.Format("View named {0} does not exists", view));
		}

		public override void GoBack()
		{
			_service.GoBack();
		}

		public override void GoForward()
		{
			_service.GoForward();
		}

		public override void ExitApplication()
		{
			Application.Current.Terminate();
		}

		#endregion
	}
}
