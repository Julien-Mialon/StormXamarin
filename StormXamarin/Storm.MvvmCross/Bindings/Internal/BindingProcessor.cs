using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore;

namespace Storm.MvvmCross.Bindings.Internal
{
	internal static class BindingProcessor
	{
		public static void ProcessBinding(object viewModel, INotifyPropertyChanged viewContext, List<BindingObject> bindingObjects)
		{
			BindingNode rootExpressionNode = new BindingNode(string.Empty);

			// add bindings created at compile time
			foreach (BindingObject bindingObject in bindingObjects)
			{
				object targetObject = bindingObject.TargetObject;
				
				foreach (BindingExpression expression in bindingObject.Expressions)
				{
					rootExpressionNode.AddExpression(expression, targetObject);
				}
			}

			// add bindings defined by attribute on the viewContext class
			IEnumerable<BindingAttribute> classAttributes = viewContext.GetType().GetCustomAttributes(typeof (BindingAttribute), true).OfType<BindingAttribute>();
			foreach (BindingAttribute attribute in classAttributes)
			{
				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding attribute on class " + viewContext.GetType());
				}
				if (string.IsNullOrEmpty(attribute.TargetPath))
				{
					throw new Exception("Target path can not be empty for binding attribute on class " + viewContext.GetType());
				}

				rootExpressionNode.AddExpression(new BindingExpression
				{
					Converter = attribute.CreateConverter(),
					ConverterParameter = attribute.ConverterParameter,
					Mode = attribute.Mode,
					UpdateEvent = "PropertyChanged",
					TargetField = attribute.TargetPath,
					SourcePath = attribute.Path,
					TargetType = BindingExpressionTargetFieldType.Undefined,
				}, viewContext);
			}

			// add bindings attached to properties in the viewContext class
			IEnumerable<PropertyInfo> properties = viewContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo property in properties)
			{
				BindingAttribute attribute = property.GetCustomAttribute<BindingAttribute>(true);

				if (attribute == null)
				{
					continue;
				}

				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on property " + property.Name + " in view context " + viewContext.GetType());
				}

				rootExpressionNode.AddExpression(new BindingExpression
				{
					Converter = attribute.CreateConverter(),
					ConverterParameter = attribute.ConverterParameter,
					Mode = attribute.Mode,
					UpdateEvent = "PropertyChanged",
					TargetField = property.Name,
					SourcePath = attribute.Path,
					TargetType = BindingExpressionTargetFieldType.Property,
				}, viewContext);
			}
			
			// add bindings attached to Events in the view context class
			IEnumerable<EventInfo> events = viewContext.GetType().GetRuntimeEvents();
			foreach (EventInfo eventInfo in events)
			{
				BindingAttribute attribute = eventInfo.GetCustomAttribute<BindingAttribute>(true);
				if (attribute == null)
				{
					continue;
				}

				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on event " + eventInfo.Name + " activity type " + viewContext.GetType());
				}
				if (attribute.Mode == BindingMode.TwoWay)
				{
					throw new Exception("BindingMode TwoWay is not supported on event " + eventInfo.Name + " activity type " + viewContext.GetType());
				}

				rootExpressionNode.AddExpression(new BindingExpression()
				{
					TargetField = eventInfo.Name,
					SourcePath = attribute.Path,
					TargetType = BindingExpressionTargetFieldType.Event,
				}, viewContext);

				rootExpressionNode.UpdateValue(viewModel);
			}
		}
	}
}
