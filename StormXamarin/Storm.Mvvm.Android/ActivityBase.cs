using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.OS;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public class ActivityBase : Activity, INotifyPropertyChanged
	{
		protected ViewModelBase ViewModel { get; private set; }

		protected Dictionary<int, List<BindingObject>> AdapterBindings { get; private set; } 

		private ActivityState _activityState = ActivityState.Uninitialized;
		private string _parametersKey;

		public ActivityBase()
		{
			AdapterBindings = new Dictionary<int, List<BindingObject>>();
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_parametersKey = Intent.GetStringExtra("key");
		}

		protected void SetViewModel(ViewModelBase viewModel)
		{
			ViewModel = viewModel;
			InitializeBindingsForAdapters();
			BindingProcessor.ProcessBinding(ViewModel, this, GetBindingPaths());
		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}

		protected virtual void InitializeBindingsForAdapters()
		{
			
		}

		public List<BindingObject> GetBindingsForAdapters(int viewId)
		{
			return AdapterBindings.ContainsKey(viewId) ? AdapterBindings[viewId] : new List<BindingObject>();
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
			_activityState = ActivityState.Running;
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
