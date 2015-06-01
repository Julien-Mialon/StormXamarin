using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
#if SUPPORT
using Android.Support.V4.App;
#else
using Android.App;
#endif
using Android.Content;
using Android.Views;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Events;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.Dialogs
{
	public abstract class AbstractDialogFragmentBase : DialogFragment, INotifyPropertyChanged, IMvvmDialog
	{
		private ActivityState _activityState = ActivityState.Uninitialized;

		public event EventHandler Canceled;
		public event EventHandler Dismissed;

		private ViewModelBase _viewModel;
		protected ViewModelBase ViewModel
		{
			get { return _viewModel; }
			private set
			{
				if (!Equals(_viewModel, value))
				{
					_viewModel = value;

					if (_activityState == ActivityState.Running && value != null)
					{
						value.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.New), ParametersKey);
					}
				}
			}
		}

		protected View RootView { get; set; }

		public string ParametersKey { get; set; }

		public FragmentManager DialogFragmentManager { get; set; }

		public void Show()
		{
			Show(DialogFragmentManager, null);
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

		#region Event Raiser parts

		protected void RaiseCanceledEvent()
		{
			this.RaiseEvent(Canceled);
		}

		protected void RaiseDismissedEvent()
		{
			this.RaiseEvent(Dismissed);
		}

		#endregion

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
			RaiseCanceledEvent();
			base.OnCancel(dialog);
		}

		public override void OnDismiss(IDialogInterface dialog)
		{
			RaiseDismissedEvent();
			base.OnDismiss(dialog);
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
			// ReSharper disable once ExplicitCallerInfoArgument
			RaisePropertyChanged(propertyName);

			return true;
		}

		#endregion
	}
}
