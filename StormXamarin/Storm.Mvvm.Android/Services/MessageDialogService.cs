using System;
using System.Collections.Generic;
using Java.Text;
using Storm.Mvvm.Dialogs;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Navigation;

namespace Storm.Mvvm.Services
{
	public class MessageDialogService : AbstractMessageDialogService, IMessageDialogService
	{
		private readonly Dictionary<string, Type> _dialogs;

		private readonly IActivityService _activityService;

		public MessageDialogService(Dictionary<string, Type> dialogs, IActivityService activityService, INavigationService navigationService) : base(navigationService)
		{
			_dialogs = dialogs;
			_activityService = activityService;
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
			fragment.Show(_activityService.CurrentActivity.FragmentManager, null);
		}
	}
}
