using System;
using System.Collections.Generic;

namespace Storm.MvvmCross.Android.Target.Compiler
{
	static class ExpressionFactory
	{
		public static Expression CreateTextExpression(string value)
		{
			return new TextExpression()
			{
				Value = value,
			};
		}

		public static Expression CreateExpression(ExpressionType type, List<Tuple<string, Expression>> attributes)
		{
			Expression expr = null;
			switch (type)
			{
				case ExpressionType.Binding:
					expr = new BindingExpression();
					break;
				case ExpressionType.Resource:
					expr = new ResourceExpression();
					break;
				case ExpressionType.Translation:
					expr = new TranslationExpression();
					break;
				case ExpressionType.Value:
					expr = new TextExpression();
					break;
			}

			if (expr == null)
			{
				throw new ArgumentOutOfRangeException("type", type, "expecting Binding, Resource, Translation or Value");
			}

			if (attributes != null)
			{
				foreach (Tuple<string, Expression> pair in attributes)
				{
					if (expr.IsCorrectKey(pair.Item1))
					{
						expr.Add(pair.Item1, pair.Item2);
					}
					else
					{
						throw new CompileException(string.Format("Unexpected attribute with key {0}", pair.Item1));
					}
				}
			}

			return expr;
		}
	}
}
