using System;
using System.Collections.Generic;
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
		
		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

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
			Type fragmentType = _dialogs[dialogKey];
			AbstractDialogFragmentBase fragment = Activator.CreateInstance(fragmentType) as AbstractDialogFragmentBase;
			if (fragment == null)
			{
				throw new Exception("Fragment does not inherit AbstractDialogFragmentBase");
			}
#if SUPPORT
			FragmentActivity fragmentActivity = ActivityService.CurrentActivity as FragmentActivity;
			if (fragmentActivity == null)
			{
				throw new Exception("Activity does not inherit ActivityBase");
			}
			fragment.DialogFragmentManager = fragmentActivity.SupportFragmentManager;
#else
			fragment.DialogFragmentManager = ActivityService.CurrentActivity.FragmentManager;
#endif
			return fragment;
		}
	}
}
