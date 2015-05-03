using System;
using System.Collections.Generic;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Navigation
{
	public abstract class AbstractMessageDialogService : IMessageDialogService
	{
		protected INavigationService NavigationService
		{
			get { return LazyResolver<INavigationService>.Service; }
		}

		public void Show(string dialogKey)
		{
			ShowDialog(dialogKey, null, null);
		}

		public void Show(string dialogKey, Dictionary<string, object> parameters)
		{
			Show(dialogKey, parameters, null);
		}

		public void Show(string dialogKey, Action dialogDismissed)
		{
			ShowDialog(dialogKey, null, dialogDismissed);
		}

		public void Show(string dialogKey, Dictionary<string, object> parameters, Action dialogDismissed)
		{
			string parametersKey = NavigationService.StoreMessageDialogParameters(dialogKey, parameters);
			ShowDialog(dialogKey, parametersKey, dialogDismissed);
		}

		public abstract void DismissCurrentDialog();

		protected abstract void ShowDialog(string dialogKey, string parametersKey, Action dialogDismissed);
	}
}
