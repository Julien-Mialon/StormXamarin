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
		private readonly List<AbstractDialogFragmentBase> _dialogStack = new List<AbstractDialogFragmentBase>(); 

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
				_dialogStack.Last().Dismiss();
			}
		}

		protected override void ShowDialog(string dialogKey, string parametersKey)
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

			_dialogStack.Add(fragment);
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

		}

		private void OnDialogDismissed(object sender, EventArgs eventArgs)
		{
			AbstractDialogFragmentBase dialog = sender as AbstractDialogFragmentBase;
			if (dialog == null)
			{
				return;
			}
			dialog.Dismissed -= OnDialogDismissed;

			_dialogStack.Remove(dialog);
		}
	}
}
