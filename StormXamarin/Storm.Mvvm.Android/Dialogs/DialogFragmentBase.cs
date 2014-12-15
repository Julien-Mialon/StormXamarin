using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm.Dialogs
{
	public abstract class DialogFragmentBase : DialogFragment, INotifyPropertyChanged
	{
		public event EventHandler PositiveButtonCommand;

		public event EventHandler NeutralButtonCommand;

		public event EventHandler NegativeButtonCommand;

		protected ViewModelBase ViewModel { get; private set; }

		protected View RootView { get; private set; }

		protected Dialog CreateDialog(Bundle savedInstanceState, string title = "", string message = "", Dictionary<DialogsButton, string> buttons = null)
		{
			RootView = CreateView(Activity.LayoutInflater, null);
			AlertDialog.Builder dialog = new AlertDialog.Builder(Activity);
			dialog.SetTitle(title);
			dialog.SetMessage(message);
			dialog.SetView(RootView);
			if (buttons != null)
			{
				if (buttons.ContainsKey(DialogsButton.Positive))
				{
					dialog.SetPositiveButton(buttons[DialogsButton.Positive], PositiveButtonAction);
				}
				if(buttons.ContainsKey(DialogsButton.Neutral))
				{
					dialog.SetNeutralButton(buttons[DialogsButton.Neutral], NeutralButtonAction);
				}
				if (buttons.ContainsKey(DialogsButton.Negative))
				{
					dialog.SetNegativeButton(buttons[DialogsButton.Negative], NegativeButtonAction);
				}
			}

			return dialog.Create();
		}

		public override void OnStart()
		{
			SetViewModel(CreateViewModel());
			base.OnStart();
		}

		protected abstract View CreateView(LayoutInflater inflater, ViewGroup container);

		protected abstract ViewModelBase CreateViewModel();

		protected void SetViewModel(ViewModelBase viewModel)
		{
			ViewModel = viewModel;
			BindingProcessor.ProcessBinding(ViewModel, this, GetBindingPaths());
		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}


		private void NegativeButtonAction(object sender, DialogClickEventArgs dialogClickEventArgs)
		{
			EventHandler handler = NegativeButtonCommand;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		private void NeutralButtonAction(object sender, DialogClickEventArgs dialogClickEventArgs)
		{
			EventHandler handler = NeutralButtonCommand;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		private void PositiveButtonAction(object sender, DialogClickEventArgs dialogClickEventArgs)
		{
			EventHandler handler = PositiveButtonCommand;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;
			RaisePropertyChanged(propertyName);

			return true;
		}

		#endregion
	}
}
