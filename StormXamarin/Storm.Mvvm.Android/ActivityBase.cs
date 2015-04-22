using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.OS;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public class ActivityBase : Activity, INotifyPropertyChanged
	{
		protected ViewModelBase ViewModel { get; private set; }

		private ActivityState _activityState = ActivityState.Uninitialized;
		private string _parametersKey;
		
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_parametersKey = Intent.GetStringExtra("key");
		}

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

		protected override void OnStart()
		{
			base.OnStart();
			AndroidContainer.GetInstance().UpdateActivity(this);
			if (ViewModel != null && _activityState != ActivityState.Running)
			{
				ViewModel.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.New), _parametersKey);
			}
			_activityState = ActivityState.Running;
		}

		protected override void OnResume()
		{
			base.OnResume();
			AndroidContainer.GetInstance().UpdateActivity(this);
			if (ViewModel != null && _activityState != ActivityState.Running)
			{
				ViewModel.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.Back), _parametersKey);
			}
			_activityState = ActivityState.Running;
		}

		protected override void OnPause()
		{
			base.OnPause();
			if (ViewModel != null && _activityState != ActivityState.Stopped)
			{
				ViewModel.OnNavigatedFrom(new NavigationArgs(NavigationArgs.NavigationMode.Forward));
			}
			_activityState = ActivityState.Stopped;
		}

		protected override void OnStop()
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

		#region Activity result handling

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			IActivityService activityService = LazyResolver<IActivityService>.Service;

			activityService.ProcessActivityResult(requestCode, resultCode, data);
		}

		#endregion
	}
}
