namespace Storm.Binding.AndroidTarget.Compiler
{
	public class ResourceExpression : Expression
	{
		public const string KEY = "Alias";

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