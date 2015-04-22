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
			ShowDialog(dialogKey, null);
		}

		public void Show(string dialogKey, Dictionary<string, object> parameters)
		{
			string parametersKey = NavigationService.StoreMessageDialogParameters(dialogKey, parameters);
			ShowDialog(dialogKey, parametersKey);
		}

		protected abstract void ShowDialog(string dialogKey, string parametersKey);
	}
}
