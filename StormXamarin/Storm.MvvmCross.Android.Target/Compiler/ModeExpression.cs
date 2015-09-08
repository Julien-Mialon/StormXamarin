using System;
using Storm.MvvmCross.Android.Target.Model;

namespace Storm.MvvmCross.Android.Target.Compiler
{
	public class ModeExpression : Expression
	{
		public BindingMode Value { get; set; }

		public override ExpressionType Type
		{
			get { return ExpressionType.BindingMode; }
		}

		protected override string ContentKey
		{
			get { throw new InvalidOperationException(); }
		}

		protected override string[] InternalAvailableKeys
		{
			get { return new string[] {}; }
		}
	}
}
