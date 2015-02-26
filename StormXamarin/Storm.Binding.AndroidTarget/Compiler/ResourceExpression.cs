namespace Storm.Binding.AndroidTarget.Compiler
{
	public class ResourceExpression : Expression
	{
		public const string KEY = "Key";

		public override ExpressionType Type
		{
			get { return ExpressionType.Resource; }
		}

		protected override string ContentKey
		{
			get { return KEY; }
		}

		protected override string[] InternalAvailableKeys
		{
			get { return new[] { KEY }; }
		}
	}
}