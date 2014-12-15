using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Storm.Mvvm.Bindings
{
	internal static class BindingProcessor
	{
		public static void ProcessBinding(ViewModelBase viewModel, object context, List<BindingObject> bindingObjects)
		{
			BindingNode rootExpressionNode = new BindingNode("");

			foreach (BindingObject bindingObject in bindingObjects)
			{
				foreach (BindingExpression expression in bindingObject.Expressions)
				{
					rootExpressionNode.AddExpression(expression, bindingObject.TargetObject);
				}
			}

			//also process expressions attached to the activity with attribute
			IEnumerable<PropertyInfo> properties = context.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<BindingAttribute>(true) != null);
			IEnumerable<EventInfo> events = context.GetType().GetEvents(BindingFlags.Public | BindingFlags.Instance).Where(x => x.GetCustomAttribute<BindingAttribute>(true) != null);
			IEnumerable<BindingElementAttribute> classAttributes = context.GetType().GetCustomAttributes<BindingElementAttribute>(true);

			foreach (PropertyInfo property in properties)
			{
				BindingAttribute attribute = property.GetCustomAttribute<BindingAttribute>(true);

				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on property " + property.Name + " activity type " + context.GetType());
				}

				rootExpressionNode.AddExpression(new BindingExpression()
				{
					Converter = attribute.GetConverter(),
					ConverterParameter = attribute.ConverterParameter,
					Mode = attribute.Mode,
					UpdateEvent = "PropertyChanged",
					TargetField = property.Name,
					SourcePath = attribute.Path,
					TargetType = BindingTargetType.Property,
				}, context);
			}

			foreach (EventInfo eventInfo in events)
			{
				BindingAttribute attribute = eventInfo.GetCustomAttribute<BindingAttribute>(true);

				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on event " + eventInfo.Name + " activity type " + context.GetType());
				}
				if (attribute.Mode == BindingMode.TwoWay)
				{
					throw new Exception("BindingMode TwoWay is not supported on event " + eventInfo.Name + " activity type " + context.GetType());
				}

				rootExpressionNode.AddExpression(new BindingExpression()
				{
					TargetField = eventInfo.Name,
					SourcePath = attribute.Path,
					TargetType = BindingTargetType.Event,
				}, context);
			}

			foreach (BindingElementAttribute attribute in classAttributes)
			{
				if (string.IsNullOrEmpty(attribute.Path))
				{
					throw new Exception("Path can not be empty for binding on class " + context.GetType());
				}
				if (string.IsNullOrEmpty(attribute.TargetPath))
				{
					throw new Exception("Target path can not be empty for binding on class " + context.GetType());
				}

				rootExpressionNode.AddExpression(new BindingExpression()
				{
					Converter = attribute.GetConverter(),
					ConverterParameter = attribute.ConverterParameter,
					Mode = attribute.Mode,
					UpdateEvent = "PropertyChanged",
					TargetField = attribute.TargetPath,
					SourcePath = attribute.Path,
					TargetType = BindingTargetType.Property,
				}, context);
			}

			rootExpressionNode.UpdateValue(viewModel);
		}
	}
}
