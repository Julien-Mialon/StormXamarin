using System.Collections.Generic;

namespace Storm.MvvmCross.Bindings
{
	public class BindingObject
	{
		public object TargetObject { get; set; }

		public List<BindingExpression> Expressions { get; set; } 
	}
}
