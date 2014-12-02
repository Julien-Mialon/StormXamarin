using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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

		private ActivityState _activityState = ActivityState.Uninitialized;
		private BindingNode _rootExpressionNode;
		private string _parametersKey;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			_parametersKey = Intent.GetStringExtra("key");
		}

		protected void SetViewModel(ViewModelBase viewModel)
		{
			ViewModel = viewModel;

			List<BindingObject> bindingObjects = GetBindingPaths();

			_rootExpressionNode = new BindingNode("");

			foreach (BindingObject bindingObject in bindingObjects)
			{
				foreach (BindingExpression expression in bindingObject.Expressions)
				{
					_rootExpressionNode.AddExpression(expression, bindingObject.TargetObject);
				}
			}

			//also process expressions attached to the activity with attribute
			IEnumerable<PropertyInfo> properties = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<BindingAttribute>(true) != null);
			IEnumerable<EventInfo> events = this.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<BindingAttribute>(true) != null);

			foreach (PropertyInfo property in properties)
			{
				BindingAttribute attribute = property.GetCustomAttribute<BindingAttribute>(true);

				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on property " + property.Name + " activity type " + this.GetType());
				}

				_rootExpressionNode.AddExpression(new BindingExpression()
				{
					Converter = attribute.GetConverter(),
					ConverterParameter = attribute.ConverterParameter,
					Mode = attribute.Mode,
					UpdateEvent = "PropertyChanged",
					TargetField = property.Name,
					SourcePath = attribute.Path,
					TargetType = BindingTargetType.Property,
				}, this);
			}

			foreach (EventInfo eventInfo in events)
			{
				BindingAttribute attribute = eventInfo.GetCustomAttribute<BindingAttribute>(true);

				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on event " + eventInfo.Name + " activity type " + this.GetType());
				}
				if (attribute.Mode == BindingMode.TwoWay)
				{
					throw new Exception("BindingMode TwoWay is not supported on event " + eventInfo.Name + " activity type " + this.GetType());
				}

				_rootExpressionNode.AddExpression(new BindingExpression()
				{
					TargetField = eventInfo.Name,
					SourcePath = attribute.Path,
					TargetType = BindingTargetType.Event,
				}, this);
			}

			_rootExpressionNode.UpdateValue(ViewModel);
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
