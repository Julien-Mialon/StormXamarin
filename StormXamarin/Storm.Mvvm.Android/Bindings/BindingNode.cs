using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Android.Provider;

namespace Storm.Mvvm.Android.Bindings
{
	class BindingNode
	{
		// Property name in the source tree (VM)
		protected string PropertyName { get; private set; }

		protected List<BindingBase> Bindings { get; private set; }

		public Dictionary<string, BindingNode> Children { get; private set; }

		protected object Value { get; private set; }

		public BindingNode(string propertyName)
		{
			PropertyName = propertyName;

			Bindings = new List<BindingBase>();
			Children = new Dictionary<string, BindingNode>();
		}

		#region Add expression into the binding tree

		public void AddExpression(BindingExpression expression, object targetObject)
		{
			AddExpression(expression, targetObject, expression.SourcePath);
		}

		private void AddExpression(BindingExpression expression, object targetObject, string propertyPath)
		{
			if (string.IsNullOrEmpty(propertyPath))
			{
				//Add it here
				Bindings.Add(BindingFactory.Create(expression, targetObject));
			}
			else
			{
				string propertyName, otherPath;
				if (propertyPath.Contains("."))
				{
					int index = propertyPath.IndexOf('.');
					propertyName = propertyPath.Substring(0, index);
					otherPath = propertyPath.Substring(index + 1);
				}
				else
				{
					otherPath = "";
					propertyName = propertyPath;
				}

				BindingNode child;
				if (Children.ContainsKey(propertyName))
				{
					child = Children[propertyName];
				}
				else
				{
					child = new BindingNode(propertyName);
					Children.Add(child.PropertyName, child);
				}

				child.AddExpression(expression, targetObject, otherPath);
			}
		}

		#endregion

		//Value is used only for PropertyChanged event 
		public void UpdateValue(object value)
		{
			if (Children.Any())
			{
				Remove(Value as INotifyPropertyChanged);
			}
			Value = value;
			if (Children.Any())
			{
				Attach(Value as INotifyPropertyChanged);
			}

			foreach (BindingBase binding in Children.SelectMany(x => x.Value.Bindings))
			{
				binding.UpdateContext(Value);
			}

			foreach (BindingBase binding in Bindings)
			{
				binding.UpdateValue(Value);
			}

			if (Value == null)
			{
				foreach (BindingNode child in Children.Values)
				{
					child.UpdateValue(null);
				}
			}
			else
			{
				foreach (BindingNode child in Children.Values)
				{
					child.UpdateValue(child.GetValue(Value));
				}
			}
		}

		protected object GetValue(object container)
		{
			PropertyInfo property = container.GetType().GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public);
			if (property != null)
			{
				return property.GetValue(container);
			}
			throw new Exception("BindingNode : can not find property " + PropertyName + " in object of type " + container.GetType());
		}

		protected void Attach(INotifyPropertyChanged notifier)
		{
			if (notifier != null)
			{
				notifier.PropertyChanged += OnPropertyChanged;
			}
		}

		protected void Remove(INotifyPropertyChanged notifier)
		{
			if (notifier != null)
			{
				notifier.PropertyChanged -= OnPropertyChanged;
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (Children.ContainsKey(e.PropertyName))
			{
				BindingNode child = Children[e.PropertyName];

				child.UpdateValue(child.GetValue(Value));
			}
		}

	}
}
