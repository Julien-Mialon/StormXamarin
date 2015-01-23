using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class NavigationService : AbstractNavigationService
	{
		private readonly Frame _rootFrame;
		private readonly Dictionary<string, Type> _views; 

		public override bool CanGoBack
		{
			get { return _rootFrame.CanGoBack; }
		}

		public override bool CanGoForward
		{
			get { return _rootFrame.CanGoForward; }
		}

		public NavigationService(Frame rootFrame, Dictionary<string, Type> views)
		{
			_views = views;
			_rootFrame = rootFrame;
		}

		public override void GoBack()
		{
			_rootFrame.GoBack();
		}

		public override void GoForward()
		{
			_rootFrame.GoForward();
		}

		public override void ExitApplication()
		{
			//TODO
		}

		protected override void RemoveBackEntry()
		{
			GoBack();
		}

		protected override void NavigateToView(string view, string parametersKey)
		{
			Type viewType = GetViewOrThrow(view);

			_rootFrame.Navigate(viewType, parametersKey);
		}

		protected virtual Type GetViewOrThrow(string view)
		{
			if (_views.ContainsKey(view))
			{
				return _views[view];
			}
			throw new Exception(string.Format("View named {0} does not exists", view));
		}
	}
}
