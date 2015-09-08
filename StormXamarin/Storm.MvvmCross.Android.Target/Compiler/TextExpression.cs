using System;

namespace Storm.MvvmCross.Android.Target.Compiler
{
	public class TextExpression : Expression
	{
		public string Value { get; set; }

		public override ExpressionType Type
		{
			get { return ExpressionType.Value; }
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