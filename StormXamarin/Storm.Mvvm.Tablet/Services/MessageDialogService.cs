using System;
using System.Collections.Generic;
using System.Linq;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class MessageDialogService : AbstractMessageDialogService
	{
		private readonly Dictionary<string, Type> _dialogs;
		private readonly List<Tuple<DialogPage, Action>> _dialogStack = new List<Tuple<DialogPage, Action>>();
		
		public MessageDialogService(Dictionary<string, Type> dialogs)
		{
			_dialogs = dialogs;
		}

		public override void DismissCurrentDialog()
		{
			if (_dialogStack.Count > 0)
			{
				_dialogStack.Last().Item1.Close();
			}
		}

		protected override void ShowDialog(string dialogKey, string parametersKey, Action dialogDismissed)
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
			dialog.Dismissed += OnDialogDismissed;

			_dialogStack.Add(new Tuple<DialogPage, Action>(dialog, dialogDismissed));
			dialog.Open(parametersKey);
		}

		private void OnDialogDismissed(object sender, EventArgs eventArgs)
		{
			DialogPage dialog = sender as DialogPage;
			if (dialog == null)
			{
				return;
			}
			dialog.Dismissed -= OnDialogDismissed;
			Tuple<DialogPage, Action> savedTuple = _dialogStack.FirstOrDefault(x => Equals(x.Item1, dialog));

			if (savedTuple != null)
			{
				_dialogStack.Remove(savedTuple);
				if (savedTuple.Item2 != null)
				{
					savedTuple.Item2();
				}
			}
		}
	}
}
