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

		public int Show(string dialogKey)
		{
			return ShowDialog(dialogKey, null, null);
		}

		public int Show(string dialogKey, Dictionary<string, object> parameters)
		{
			return Show(dialogKey, parameters, null);
		}

		public int Show(string dialogKey, Action dialogDismissed)
		{
			return ShowDialog(dialogKey, null, dialogDismissed);
		}

		public int Show(string dialogKey, Dictionary<string, object> parameters, Action dialogDismissed)
		{
			string parametersKey = NavigationService.StoreMessageDialogParameters(dialogKey, parameters);
			return ShowDialog(dialogKey, parametersKey, dialogDismissed);
		}

		public abstract void DismissCurrentDialog();

		public abstract void DismissDialog(int id);

		protected abstract int ShowDialog(string dialogKey, string parametersKey, Action dialogDismissed);
	}
}
