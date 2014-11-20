using System.Collections.Generic;

namespace Storm.Mvvm.Android.Bindings
{
	public class BindingObject
	{
		public string TargetObjectName { get; set; }

		public List<BindingExpression> Expressions { get; private set; }

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
			Expressions.Add(expr);
		}
	}
}
