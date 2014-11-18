using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows.Data;

namespace Storm.Mvvm.Android.Bindings
{
	public class BindingExpression
	{
		#region Fields with generated value from xml view file 

		public string TargetField { get; set; }

		public string SourcePath { get; set; }

		public IValueConverter Converter { get; set; }

		public string ConverterParameter { get; set; }

		public BindingMode Mode { get; set; }

		public string UpdateEvent { get; set; }

		#endregion

		#region Runtime fields

		public PropertyInfo TargetPropertyHandler { get; set; }

		public EventInfo TargetEventHandler { get; set; }

		public EventInfo UpdateEventHandler { get; set; }

		public BindingObject BindingObject { get; set; }

		private object _sourceContext;
		public object SourceContext
		{
			get { return _sourceContext; }
			set
			{
				if (!object.Equals(_sourceContext, value))
				{
					_sourceContext = value;

					int lastIndex = SourcePath.LastIndexOf('.');
					if (lastIndex >= 0)
					{
						lastIndex++;
					}
					else
					{
						lastIndex = 0;
					}
					string lastPath = SourcePath.Substring(lastIndex);
					PropertyInfo property = _sourceContext.GetType().GetProperty(lastPath, BindingFlags.Public | BindingFlags.Instance);
					if (property == null)
					{
						//ERROR
						SourceProperty = null;
						throw new Exception("Binding issue, can not access property " + lastPath + " in object of type " + _sourceContext.GetType());
					}
					else
					{
						SourceProperty = property;
					}
				}
			}
		}

		public PropertyInfo SourceProperty { get; set; }

		public bool IsPropertyAttached
		{
			get { return TargetPropertyHandler != null; }
		}

		public bool IsEventAttached
		{
			get { return TargetEventHandler != null; }
		}

		public EventToCommandHelper EventHelper { get; set; }
		public TwoWayHelper TwoWayEventHelper { get; set; }

		#endregion

		public BindingExpression()
		{
			
		}

		public BindingExpression(string targetField, string sourcePath)
		{
			this.TargetField = targetField;
			this.SourcePath = sourcePath;
		}
	}

	public class BindingObject
	{
		public string TargetObjectName { get; set; }

		public List<BindingExpression> Expressions { get; private set; }

		public object TargetObject { get; set; }

		public BindingObject()
		{
			Expressions = new List<BindingExpression>();
		}

		public BindingObject(string objectId) : this()
		{
			TargetObjectName = objectId;
		}

		public void AddExpression(BindingExpression expr)
		{
			this.Expressions.Add(expr);
		}
	}

	public enum BindingMode
	{
		OneTime,
		OneWay,
		TwoWay,
	}

	public class BindingProcessor
	{
		
	}
}
