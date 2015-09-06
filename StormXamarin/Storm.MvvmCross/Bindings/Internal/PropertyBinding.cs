using System;
using System.Globalization;
using System.Reflection;

namespace Storm.MvvmCross.Bindings.Internal
{
	/// <summary>
	/// This class handle proccess of updating value for OneWay Binding
	/// </summary>
	class PropertyBinding : BindingBase
	{
		protected PropertyInfo TargetProperty { get; private set; }

		protected DependencyPropertyProxy TargetProxy { get; private set; }

		public PropertyBinding(BindingExpression expression, object targetObject) : base(expression, targetObject)
		{
			
		}

		public override void Initialize()
		{
			TargetProperty = TargetObject.GetType().GetPropertyForBinding(Expression.TargetField);

			if (TargetProperty == null)
			{
				throw new Exception("PropertyBinding : no property named " + Expression.TargetField + " in object of type " + TargetObject.GetType());
			}

			TargetProxy = GetTargetProxy();
		}

		public override void UpdateValue(object value)
		{
			if (TargetProperty == null)
			{
				throw new Exception("PropertyBinding has not been initialized");
			}

			if (Expression.Converter != null)
			{
				value = Expression.Converter.Convert(value, TargetProperty.PropertyType, Expression.ConverterParameter, CultureInfo.CurrentUICulture);
			}
			TargetProxy.SetValue(value);
		}

		protected virtual DependencyPropertyProxy GetTargetProxy()
		{
			return new DependencyPropertyProxy(TargetObject, TargetProperty);
		}
	}
}
