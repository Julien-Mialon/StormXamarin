using System;
using System.Collections.Generic;
using System.Linq;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class MessageDialogService : AbstractMessageDialogService
	{
		private readonly Dictionary<string, Type> _dialogs;
		private readonly List<Tuple<int, DialogPage, Action>> _dialogStack = new List<Tuple<int, DialogPage, Action>>();

		private int _dialogId = 1;

		public MessageDialogService(Dictionary<string, Type> dialogs)
		{
			_dialogs = dialogs;
		}

		public override void DismissCurrentDialog()
		{
			if (_dialogStack.Count > 0)
			{
				_dialogStack.Last().Item2.Close();
			}
		}

		public override void DismissDialog(int id)
		{
			Tuple<int, DialogPage, Action> dialog = _dialogStack.FirstOrDefault(x => x.Item1 == id);

			if(dialog != null)
			{
				dialog.Item2.Close();
			}
		}

		protected override int ShowDialog(string dialogKey, string parametersKey, Action dialogDismissed)
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

			int id = _dialogId++;
			_dialogStack.Add(new Tuple<int, DialogPage, Action>(id, dialog, dialogDismissed));
			dialog.Open(parametersKey);

			return id;
		}

		private void OnDialogDismissed(object sender, EventArgs eventArgs)
		{
			DialogPage dialog = sender as DialogPage;
			if (dialog == null)
			{
				return;
			}
			dialog.Dismissed -= OnDialogDismissed;
			Tuple<int, DialogPage, Action> savedTuple = _dialogStack.FirstOrDefault(x => Equals(x.Item1, dialog));

			if (savedTuple != null)
			{
				_dialogStack.Remove(savedTuple);
				if (savedTuple.Item3 != null)
				{
					savedTuple.Item3();
				}
			}
		}
	}
}
