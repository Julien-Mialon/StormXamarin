using System.Collections.Generic;

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

		protected override Dictionary<string, IEnumerable<ExpressionType>> GetExpectedValueType()
		{
			return new Dictionary<string, IEnumerable<ExpressionType>>
			{
				{KEY, new List<ExpressionType> {ExpressionType.Value}},
			};
		}

		protected override bool CheckConstraints()
		{
			if (!Has(KEY))
			{
				BindingPreprocess.Logger.LogError("Key is mandatory in Resource expression");
				return false;
			}
			return true;
		}
	}
}