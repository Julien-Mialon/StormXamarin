using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Storm.Mvvm.Android.Bindings
{
	public class TwoWayHelper
	{
		public static MethodInfo EventMethodInfo = null;

		private BindingExpression _expression;

		static TwoWayHelper()
		{
			Type type = typeof(TwoWayHelper);
			EventMethodInfo = type.GetMethod("Trigger", BindingFlags.Instance | BindingFlags.Public);
		}

		public TwoWayHelper()
		{
			
		}

		public TwoWayHelper(BindingExpression expression)
		{
			_expression = expression;
		}

		public void Trigger(object sender, EventArgs e)
		{
			if (_expression != null)
			{
				if (_expression.SourceContext != null && _expression.SourceProperty != null)
				{
					object updatedValue = _expression.TargetPropertyHandler.GetValue(_expression.BindingObject.TargetObject);
					_expression.SourceProperty.SetValue(_expression.SourceContext, updatedValue);
				}
			}
		}
	}
}
