using System;
using System.Collections.Generic;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class MessageDialogService : AbstractMessageDialogService
	{
		private readonly Dictionary<string, Type> _dialogs;

		public MessageDialogService(Dictionary<string, Type> dialogs)
		{
			_dialogs = dialogs;
		}

		protected override IMvvmDialog ShowDialog(string dialogKey)
		{
			if (!_dialogs.ContainsKey(dialogKey))
			{
				throw new ArgumentException("DialogKey does not exists");
			}
			Type dialogType = _dialogs[dialogKey];
			DialogPage dialog = Activator.CreateInstance(dialogType) as DialogPage;
			if (dialog == null)
			{
				throw new Exception("Dialog does not inherit DialogPage");
			}
			return dialog;
		}
	}
}
