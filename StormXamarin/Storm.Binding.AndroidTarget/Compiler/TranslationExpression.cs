using System.Collections.Generic;

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

		protected override Dictionary<string, IEnumerable<ExpressionType>> GetExpectedValueType()
		{
			return new Dictionary<string, IEnumerable<ExpressionType>>
			{
				{UID, new List<ExpressionType> {ExpressionType.Value}},
				{KEY, new List<ExpressionType> {ExpressionType.Value}},
			};
		}

		protected override bool CheckConstraints()
		{
			if (!Has(KEY))
			{
				BindingPreprocess.Logger.LogError("Key is mandatory in Translation expression");
				return false;
			}
			return true;
		}
	}
}