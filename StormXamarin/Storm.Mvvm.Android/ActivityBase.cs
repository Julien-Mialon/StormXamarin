using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Android.App;
using Android.OS;
using Storm.Mvvm.Android.Bindings;

namespace Storm.Mvvm.Android
{
	public class ActivityBase : Activity
	{
		protected ViewModelBase ViewModel { get; private set; }

		

		protected void SetViewModel(ViewModelBase viewModel, Type idContainerType)
		{
			ViewModel = viewModel;

			List<BindingObject> bindingObjects = GetBindingPaths();

			//Get ids values 
			Dictionary<string, int> ids = idContainerType.GetRuntimeFields().Where(field => field.IsLiteral).ToDictionary(field => field.Name, field => (int)field.GetRawConstantValue());

			foreach (BindingObject bindingObject in bindingObjects)
			{
				bindingObject.TargetObject = FindViewById(ids[bindingObject.TargetObjectName]);
				if (bindingObject.TargetObject != null)
				{
					Type targetType = bindingObject.TargetObject.GetType();
					//Process all expressions
					foreach (BindingExpression expression in bindingObject.Expressions)
					{
						try
						{
							//if a property exists, use it
							PropertyInfo property = targetType.GetProperty(expression.TargetField, BindingFlags.Public | BindingFlags.IgnoreCase);
							if (property != null)
							{
								expression.TargetPropertyHandler = property;
								continue;
							}

							//otherwise supposed its an event
							EventInfo ev = targetType.GetEvent(expression.TargetField, BindingFlags.Public | BindingFlags.IgnoreCase);
							if (ev != null)
							{
								expression.TargetEventHandler = ev;
								continue;
							}
						}
						catch (Exception)
						{
							//otherwise, an error happened, so throw an exception
							throw;
						}

					}
				}
			}

			List<BindingExpression> expressions = bindingObjects.SelectMany(x => x.Expressions)
																.OrderBy(x => x.SourcePath)
																.ToList();

		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}
	}
}
