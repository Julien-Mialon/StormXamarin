using System;
using System.Collections.Generic;

namespace Storm.Mvvm.Services
{
	public interface IMessageDialogService
	{
		int Show(string dialogKey);

		int Show(string dialogKey, Dictionary<string, object> parameters);

		int Show(string dialogKey, Action dialogDismissed);

		int Show(string dialogKey, Dictionary<string, object> parameters, Action dialogDismissed);

		void DismissCurrentDialog();

		void DismissDialog(int id);
	}
}
