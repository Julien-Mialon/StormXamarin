using System;
using System.Collections.Generic;

namespace Storm.Mvvm.Services
{
	public interface IMessageDialogService
	{
		void Show(string dialogKey);

		void Show(string dialogKey, Dictionary<string, object> parameters);

		void Show(string dialogKey, Action dialogDismissed);

		void Show(string dialogKey, Dictionary<string, object> parameters, Action dialogDismissed);

		void DismissCurrentDialog();
	}
}
