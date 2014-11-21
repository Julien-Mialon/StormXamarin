using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.OS;
using Android.Views;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public class ActivityBase : Activity
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

		protected void SetViewModel(ViewModelBase viewModel, Type idContainerType)
		{
			ViewModel = viewModel;

			List<BindingObject> bindingObjects = GetBindingPaths();

			//Get ids values 
			Dictionary<string, View> views = idContainerType.GetRuntimeFields().Where(field => field.IsLiteral).ToDictionary(field => field.Name, field => FindViewById((int)field.GetRawConstantValue()));

			_rootExpressionNode = new BindingNode("");

			foreach (BindingObject bindingObject in bindingObjects)
			{
				if (views.ContainsKey(bindingObject.TargetObjectName))
				{
					object targetView = views[bindingObject.TargetObjectName];
					foreach (BindingExpression expression in bindingObject.Expressions)
					{
						_rootExpressionNode.AddExpression(expression, targetView);
					}
				}
				else
				{
					throw new Exception("ActivityBase : can not access view element " + bindingObject.TargetObjectName);
				}
			}

			_rootExpressionNode.UpdateValue(ViewModel);
		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}

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
	}
}
