namespace Storm.Binding.AndroidTarget.Compiler
{
	public class TranslationExpression : Expression
	{
		public const string UID = "Uid";
		public const string KEY = "Key";

		public override ExpressionType Type
		{
			get { return ExpressionType.Translation; }
		}

		protected override string ContentKey
		{
			get { return KEY; }
		}

		protected override string[] InternalAvailableKeys
		{
			get { return new[] { UID, KEY }; }
		}
	}
}