using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Views;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm.Dialogs
{
	public abstract class AbstractDialogFragmentBase : DialogFragment, INotifyPropertyChanged
	{
		public event EventHandler CancelEvent;

		protected ViewModelBase ViewModel { get; set; }

		protected View RootView { get; set; }

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

		public override void OnCancel(IDialogInterface dialog)
		{
			RaiseCancelEvent();
			base.OnCancel(dialog);
		}

		public override void OnStart()
		{
			SetViewModel(CreateViewModel());
			base.OnStart();
		}

		protected void RaiseCancelEvent()
		{
			EventHandler handler = CancelEvent;
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
