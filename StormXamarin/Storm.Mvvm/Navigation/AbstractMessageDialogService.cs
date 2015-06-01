using System;
using System.Collections.Generic;
using System.Linq;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Navigation
{
	public abstract class AbstractMessageDialogService : IMessageDialogService
	{
		private readonly List<Tuple<IMvvmDialog, Action, int>> _dialogStack = new List<Tuple<IMvvmDialog, Action, int>>();
		private int _dialogId = 1;


		protected INavigationService NavigationService
		{
			get { return LazyResolver<INavigationService>.Service; }
		}

		public void DismissCurrentDialog()
		{
			if (_dialogStack.Count > 0)
			{
				_dialogStack.Last().Item1.Dismiss();
			}
		}

		public void DismissDialog(int id)
		{
			Tuple<IMvvmDialog, Action, int> dialog = _dialogStack.FirstOrDefault(x => x.Item3 == id);

			if (dialog != null)
			{
				dialog.Item1.Dismiss();
			}
		}

		public int Show(string dialogKey)
		{
			return Show(dialogKey, null, null);
		}

		public int Show(string dialogKey, Dictionary<string, object> parameters)
		{
			return Show(dialogKey, parameters, null);
		}

		public int Show(string dialogKey, Action dialogDismissed)
		{
			return Show(dialogKey, null, dialogDismissed);
		}

		public int Show(string dialogKey, Dictionary<string, object> parameters, Action dialogDismissed)
		{
			int id = _dialogId++;
			parameters = parameters ?? new Dictionary<string, object>();
			parameters.Add("DialogId", id);
			string parametersKey = NavigationService.StoreMessageDialogParameters(dialogKey, parameters);

			IMvvmDialog dialog = ShowDialog(dialogKey);

			dialog.ParametersKey = parametersKey;
			dialog.Dismissed += OnDialogDismissed;

			_dialogStack.Add(new Tuple<IMvvmDialog, Action, int>(dialog, dialogDismissed, id));
			dialog.Show();
			return id;
		}

		private void OnDialogDismissed(object sender, EventArgs eventArgs)
		{
			IMvvmDialog dialog = sender as IMvvmDialog;
			if (dialog == null)
			{
				return;
			}
			dialog.Dismissed -= OnDialogDismissed;
			Tuple<IMvvmDialog, Action, int> savedTuple = _dialogStack.FirstOrDefault(x => Equals(x.Item1, dialog));

			if (savedTuple != null)
			{
				_dialogStack.Remove(savedTuple);
				if (savedTuple.Item2 != null)
				{
					savedTuple.Item2();
				}
			}
		}

		protected abstract IMvvmDialog ShowDialog(string dialogKey);
	}
}
