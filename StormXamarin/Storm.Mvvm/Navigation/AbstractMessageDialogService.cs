using System.Collections.Generic;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Navigation
{
	public abstract class AbstractMessageDialogService : IMessageDialogService
	{
		private readonly INavigationService _navigationService;

		protected AbstractMessageDialogService(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		public void Show(string dialogKey)
		{
			ShowDialog(dialogKey, null);
		}

		public void Show(string dialogKey, Dictionary<string, object> parameters)
		{
			string parametersKey = _navigationService.StoreMessageDialogParameters(dialogKey, parameters);
			ShowDialog(dialogKey, parametersKey);
		}

		protected abstract void ShowDialog(string dialogKey, string parametersKey);
	}
}
