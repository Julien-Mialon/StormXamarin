using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Views;
using Storm.Mvvm.Android.Bindings;

namespace Storm.Mvvm.Android
{
	public class ActivityBase : Activity
	{
		protected ViewModelBase ViewModel { get; private set; }

		private BindingNode _rootExpressionNode;

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
	}
}
