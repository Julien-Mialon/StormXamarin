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
	}

	public class BindingObject
	{
		public string TargetObjectName { get; set; }

		public IEnumerable<BindingExpression> Expressions { get; set; }

		public object TargetObject { get; set; }
	}

	public class BindingProcessor
	{
		
	}
}
