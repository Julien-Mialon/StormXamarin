using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Views;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Dialogs
{
	public abstract class AbstractDialogFragmentBase : DialogFragment, INotifyPropertyChanged
	{
		private ActivityState _activityState = ActivityState.Uninitialized;

		public event EventHandler CancelEvent;

		protected ViewModelBase ViewModel { get; set; }

		protected View RootView { get; set; }

		public string ParametersKey { get; set; }


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
		
		protected void RaiseCancelEvent()
		{
			EventHandler handler = CancelEvent;
			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}
		
		#region Lifecycle implementation

		public override void OnStart()
		{
			base.OnStart();
			SetViewModel(CreateViewModel());
			if (ViewModel != null && _activityState != ActivityState.Running)
			{
				ViewModel.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.New), ParametersKey);
			}
			_activityState = ActivityState.Running;
		}

		public override void OnResume()
		{
			base.OnResume();
			if (ViewModel != null && _activityState != ActivityState.Running)
			{
				ViewModel.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.Back), ParametersKey);
			}
			_activityState = ActivityState.Running;
		}

		public override void OnPause()
		{
			base.OnPause();
			if (ViewModel != null && _activityState != ActivityState.Stopped)
			{
				ViewModel.OnNavigatedFrom(new NavigationArgs(NavigationArgs.NavigationMode.Forward));
			}
			_activityState = ActivityState.Stopped;
		}

		public override void OnStop()
		{
			base.OnStop();
			if (ViewModel != null && _activityState != ActivityState.Stopped)
			{
				ViewModel.OnNavigatedFrom(new NavigationArgs(NavigationArgs.NavigationMode.Back));
			}
			_activityState = ActivityState.Stopped;
		}

		public override void OnCancel(IDialogInterface dialog)
		{
			RaiseCancelEvent();
			base.OnCancel(dialog);
		}

		#endregion

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
