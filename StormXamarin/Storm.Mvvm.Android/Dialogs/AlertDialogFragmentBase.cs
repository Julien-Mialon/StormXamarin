using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Storm.Mvvm.Events;

namespace Storm.Mvvm.Dialogs
{
	public abstract class AlertDialogFragmentBase : AbstractDialogFragmentBase
	{
		private string _title;
		private string _message;
		private readonly Dictionary<DialogsButton, string> _buttons = new Dictionary<DialogsButton, string>(); 

		public event EventHandler PositiveButtonEvent;
		public event EventHandler NeutralButtonEvent;
		public event EventHandler NegativeButtonEvent;

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		public string Message
		{
			get { return _message; }
			set { _message = value; }
		}

		public Dictionary<DialogsButton, string> Buttons
		{
			get { return _buttons; }
		}

		public override Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			RootView = CreateView(Activity.LayoutInflater, null);
			AlertDialog.Builder dialog = new AlertDialog.Builder(Activity);
			dialog.SetTitle(_title);
			dialog.SetMessage(_message);
			dialog.SetView(RootView);
			if (_buttons != null)
			{
				if (_buttons.ContainsKey(DialogsButton.Positive))
				{
					dialog.SetPositiveButton(_buttons[DialogsButton.Positive], (sender, args) => RaisePositiveButtonEvent());
				}
				if (_buttons.ContainsKey(DialogsButton.Neutral))
				{
					dialog.SetNeutralButton(_buttons[DialogsButton.Neutral], (sender, args) => RaiseNeutralButtonEvent());
				}
				if (_buttons.ContainsKey(DialogsButton.Negative))
				{
					dialog.SetNegativeButton(_buttons[DialogsButton.Negative], (sender, args) => RaiseNegativeButtonEvent());
				}
			}
			return dialog.Create();
		}
		
		protected void RaisePositiveButtonEvent()
		{
			this.RaiseEvent(PositiveButtonEvent);
		}

		protected void RaiseNeutralButtonEvent()
		{
			this.RaiseEvent(NeutralButtonEvent);
		}

		protected void RaiseNegativeButtonEvent()
		{
			this.RaiseEvent(NegativeButtonEvent);
		}
	}
}
