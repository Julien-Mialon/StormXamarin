using System;
using System.Globalization;
using System.Reflection;

namespace Storm.MvvmCross.Bindings.Internal
{
	class TwoWayPropertyBinding : PropertyBinding
	{
		protected PropertyInfo SourceProperty { get; private set; }
		protected string SourcePropertyName { get; private set; }


		public TwoWayPropertyBinding(BindingExpression expression, object targetObject)
			: base(expression, targetObject)
		{
			if (string.IsNullOrEmpty(expression.UpdateEvent))
			{
				throw new Exception("TwoWayPropertyBinding : can not update in two way mode without an event");
			}
		}

		public override void Initialize()
		{
			base.Initialize();

			int lastIndex = Expression.SourcePath.LastIndexOf('.');
			if (lastIndex >= 0)
			{
				lastIndex++;
			}
			else
			{
				lastIndex = 0;
			}

			SourcePropertyName = Expression.SourcePath.Substring(lastIndex);
		}

		public override void UpdateContext(object context)
		{
			base.UpdateContext(context);

			if (DataContext != null)
			{
				SourceProperty = DataContext.GetType().GetPropertyForBinding(SourcePropertyName);

				if (SourceProperty == null)
				{
					throw new Exception("TwoWayPropertyBinding : can not find property " + SourcePropertyName + " in object of type " + DataContext.GetType());
				}
			}
		}

		protected override DependencyPropertyProxy GetTargetProxy()
		{
			EventInfo updateEvent = TargetObject.GetType().GetEventForBinding(Expression.UpdateEvent);

			DependencyPropertyProxy proxy = new DependencyPropertyProxy(TargetObject, TargetProperty, updateEvent);
			proxy.OnPropertyChanged += ProxyOnOnPropertyChanged;
			proxy.Attach();

			return proxy;
		}

		private void ProxyOnOnPropertyChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			if (SourceProperty != null && DataContext != null)
			{
				object value = e.NewValue;
				if (Expression.Converter != null)
				{
					value = Expression.Converter.ConvertBack(value, SourceProperty.PropertyType, Expression.ConverterParameter, CultureInfo.CurrentUICulture);
				}
				value = BindingHelper.TypeConverterService.ConvertToType(value, SourceProperty.PropertyType);
				
				if (SourceProperty.CanWrite)
				{
					SourceProperty.SetValue(DataContext, value);
				}
			}
		}
	}
}
