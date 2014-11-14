using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Storm.Mvvm.Android.Bindings
{
	public class BindingExpression
	{
		public string TargetField { get; set; }

		public string SourcePath { get; set; }

		public PropertyInfo TargetPropertyHandler { get; set; }

		public EventInfo TargetEventHandler { get; set; }

		public BindingObject BindingObject { get; set; }

		public bool IsPropertyAttached
		{
			get { return TargetPropertyHandler != null; }
		}

		public bool IsEventAttached
		{
			get { return TargetEventHandler != null; }
		}

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

	public class BindingProcessor
	{
		
	}
}
