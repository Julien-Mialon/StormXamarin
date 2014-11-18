using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Input;

namespace Storm.Mvvm.Android.Bindings
{
	public class ExpressionNode
	{
		public string PropertyName { get; set; }

		public List<BindingExpression> Expressions { get; private set; }

		public Dictionary<string, ExpressionNode> Children { get; private set; }

		public ExpressionNode()
		{
			Expressions = new List<BindingExpression>();
			Children = new Dictionary<string, ExpressionNode>();
		}

		// Attach to INotifyPropertyChanged.PropertyChanged event the correct one
		private void AttachToEvent(object context)
		{
			if (Children.Any())
			{
				INotifyPropertyChanged notifier = context as INotifyPropertyChanged;
				if (notifier == null)
				{
					return;
				}

				notifier.PropertyChanged += OnPropertyChanged;
			}
		}

		//Set for all expressions the value in parameter
		private void UpdateValue(object value)
		{
			foreach (BindingExpression expression in Expressions)
			{
				if (expression.IsEventAttached)
				{
					if (value == null || value is ICommand)
					{
						if (expression.EventHelper == null)
						{
							expression.EventHelper = new EventToCommandHelper();

							Type delegateType = expression.TargetEventHandler.EventHandlerType;
							Delegate handler = Delegate.CreateDelegate(delegateType, expression.EventHelper, EventToCommandHelper.EventMethodInfo);

							MethodInfo addHandler = expression.TargetEventHandler.GetAddMethod();
							addHandler.Invoke(expression.BindingObject.TargetObject, new object[] {handler});
						}

						expression.EventHelper.Command = value as ICommand;
					}
					else
					{
						throw new Exception("Type other than ICommand are not supported for event binding");
					}
				}
				else if(expression.IsPropertyAttached)
				{
					if (expression.Converter != null)
					{
						value = expression.Converter.Convert(value, expression.TargetPropertyHandler.PropertyType, expression.ConverterParameter, CultureInfo.CurrentCulture);
					}
					expression.TargetPropertyHandler.SetValue(expression.BindingObject.TargetObject, value);
				}
			}
		}

		// Get the value of the property concerned by this node from the context
		private object GetValue(object context)
		{
			try
			{
				PropertyInfo property = context.GetType().GetProperty(PropertyName, BindingFlags.Public | BindingFlags.Instance);
				if (property != null)
				{
					return property.GetValue(context);
				}
			}
			catch (Exception)
			{

			}
			throw new Exception("BindingExpression Error : can not access path : " + PropertyName + " in object of type " + context.GetType());
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Children.ContainsKey(e.PropertyName))
			{
				ExpressionNode child = Children[e.PropertyName];
				child.FullUpdate(child.GetValue(sender));
			}
		}

		public void FullUpdate(object context)
		{
			if (context == null)
			{
				return;
			}

			if (Children.Any())
			{
				this.AttachToEvent(context);
			}

			if (!string.IsNullOrWhiteSpace(this.PropertyName))
			{
				this.UpdateValue(context);
			}
			// root node, just attach the event and go to children
			foreach (ExpressionNode node in Children.Values)
			{
				foreach (BindingExpression expr in node.Expressions)
				{
					expr.SourceContext = context;
				}
				node.FullUpdate(node.GetValue(context));
			}
		}

		public void Initialize()
		{
			foreach(BindingExpression expr in Expressions.Where(x => x.UpdateEventHandler != null))
			{
				if (expr.TwoWayEventHelper == null)
				{
					expr.TwoWayEventHelper = new TwoWayHelper(expr);
				}

				Type delegateType = expr.UpdateEventHandler.EventHandlerType;
				Delegate handler = Delegate.CreateDelegate(delegateType, expr.TwoWayEventHelper, TwoWayHelper.EventMethodInfo);

				MethodInfo addHandler = expr.UpdateEventHandler.GetAddMethod();
				addHandler.Invoke(expr.BindingObject.TargetObject, new object[] {handler});
			}

			foreach (ExpressionNode node in Children.Values)
			{
				node.Initialize();
			}
		}
	}
}
