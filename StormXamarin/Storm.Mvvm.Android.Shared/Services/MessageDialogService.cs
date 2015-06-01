using System;
using System.Collections.Generic;
using System.Linq;
#if SUPPORT
using Android.Support.V4.App;
#endif
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class MessageDialogService : AbstractMessageDialogService, IMessageDialogService
	{
		private readonly Dictionary<string, Type> _dialogs;
		private readonly List<Tuple<AbstractDialogFragmentBase, Action, int>> _dialogStack = new List<Tuple<AbstractDialogFragmentBase, Action, int>>();

		private int _dialogId = 1;

		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		public MessageDialogService(Dictionary<string, Type> dialogs)
		{
			_dialogs = dialogs;
		}

		public override void DismissCurrentDialog()
		{
			if (_dialogStack.Count > 0)
			{
				_dialogStack.Last().Item1.Dismiss();
			}
		}

		public override void DismissDialog(int id)
		{
			Tuple<AbstractDialogFragmentBase, Action, int> dialog = _dialogStack.FirstOrDefault(x => x.Item3 == id);

			if(dialog != null)
			{
				dialog.Item1.Dismiss();
			}
		}

		protected override int ShowDialog(string dialogKey, string parametersKey, Action dialogDismissed)
		{
			if (!_dialogs.ContainsKey(dialogKey))
			{
				throw new ArgumentException("DialogKey does not exists");
			}
			Type fragmentType = _dialogs[dialogKey];
			AbstractDialogFragmentBase fragment = Activator.CreateInstance(fragmentType) as AbstractDialogFragmentBase;
			if (fragment == null)
			{
				throw new Exception("Fragment does not inherit AbstractDialogFragmentBase");
			}
			fragment.ParametersKey = parametersKey;
			fragment.Dismissed += OnDialogDismissed;

			int id = _dialogId++;
			_dialogStack.Add(new Tuple<AbstractDialogFragmentBase, Action, int>(fragment, dialogDismissed, id));
#if SUPPORT
			FragmentActivity fragmentActivity = ActivityService.CurrentActivity as FragmentActivity;
			if (fragmentActivity == null)
			{
				throw new Exception("Activity does not inherit ActivityBase");
			}

			fragment.Show(fragmentActivity.SupportFragmentManager, null);
#else
			fragment.Show(ActivityService.CurrentActivity.FragmentManager, null);
#endif
			return id;
		}

		private void OnDialogDismissed(object sender, EventArgs eventArgs)
		{
			AbstractDialogFragmentBase dialog = sender as AbstractDialogFragmentBase;
			if (dialog == null)
			{
				return;
			}
			dialog.Dismissed -= OnDialogDismissed;
			Tuple<AbstractDialogFragmentBase, Action, int> savedTuple = _dialogStack.FirstOrDefault(x => Equals(x.Item1, dialog));

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
