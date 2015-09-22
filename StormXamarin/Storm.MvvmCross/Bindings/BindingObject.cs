using System.Collections.Generic;

namespace Storm.MvvmCross.Bindings
{
	public class BindingObject
	{
		public object TargetObject { get; set; }

		public List<BindingExpression> Expressions { get; set; }

		public BindingObject()
		{
			Expressions = new List<BindingExpression>();
		}

		public BindingObject(object target) : this()
		{
			TargetObject = target;
		}
	}
}
