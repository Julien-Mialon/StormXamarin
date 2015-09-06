using System;
using System.Reflection;

namespace Storm.MvvmCross.Bindings.Internal
{
	static class BindingFactory
	{
		public static BindingBase Create(BindingExpression expression, object targetObject)
		{
			Type targetType = targetObject.GetType();
			
			EventInfo eventInfo = targetType.GetEventForBinding(expression.TargetField);
			if (eventInfo != null)
			{
				BindingBase result = new EventBinding(expression, targetObject);
				result.Initialize();
				return result;
			}

			PropertyInfo propertyInfo = targetType.GetPropertyForBinding(expression.TargetField);
			if (propertyInfo != null)
			{
				BindingBase result;
				switch (expression.Mode)
				{
					case BindingMode.OneWay:
						result = new PropertyBinding(expression, targetObject);
						break;
					case BindingMode.TwoWay:
						result = new TwoWayPropertyBinding(expression, targetObject);
						break;
					//case BindingMode.OneTime:
					//case BindingMode.OneWayToSource:
					default:
						// TODO implement other binding modes
						throw new NotImplementedException();	
				}
				result.Initialize();
				return result;
			}
			
			throw new Exception("BindingFactory : can not find member " + expression.TargetField + " in object of type " + targetObject.GetType());
		}
	}
}
