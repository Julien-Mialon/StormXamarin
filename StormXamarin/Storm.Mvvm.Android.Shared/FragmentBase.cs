using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.OS;
using Android.Views;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Services;
#if SUPPORT
using Android.Support.V4.App;
#else
using Android.App;
#endif

namespace Storm.Mvvm
{
	public abstract class FragmentBase : Fragment, INotifyPropertyChanged
	{
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

		protected View RootView { get; private set; }

		public string ParametersKey { get; set; }

		private ActivityState _activityState = ActivityState.Uninitialized;


		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (RootView != null)
			{
				ViewGroup parent = (ViewGroup)RootView.Parent;
				parent.RemoveView(RootView);
			}
			else
			{
				RootView = CreateView(inflater, container);
			}

			return RootView;
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
			// ReSharper disable once ExplicitCallerInfoArgument : need it here
			RaisePropertyChanged(propertyName);

			return true;
		}

		#endregion
	}
}
