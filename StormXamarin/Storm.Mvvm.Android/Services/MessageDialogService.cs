using System;
using System.Collections.Generic;
using System.Text;
using Android.App;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Services
{
	public class MessageDialogService : AbstractServiceWithActivity, IMessageDialogService
	{
		private readonly Dictionary<string, Type> _dialogs; 

		public MessageDialogService(Dictionary<string, Type> dialogs, IActivityService activityService) : base(activityService)
		{
			_dialogs = dialogs;
		}

		public void Show(string dialogKey)
		{
			if (!_dialogs.ContainsKey(dialogKey))
			{
				throw new ArgumentException("DialogKey does not exists");
			}
			Type fragmentType = _dialogs[dialogKey];
			DialogFragmentBase fragment = Activator.CreateInstance(fragmentType) as DialogFragmentBase;
			if (fragment == null)
			{
				throw new Exception("Fragment does not inherit DialogFragmentBase");
			}
			fragment.Show(CurrentActivity.FragmentManager, null);
		}

		public void Show(string dialogKey, Dictionary<string, object> parameters)
		{
			//TODO : implement it
			throw new NotImplementedException();
		}
	}
}
