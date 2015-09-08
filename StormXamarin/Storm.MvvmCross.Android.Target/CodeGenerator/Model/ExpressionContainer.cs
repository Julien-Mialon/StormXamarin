using Storm.MvvmCross.Android.Target.Compiler;

namespace Storm.MvvmCross.Android.Target.CodeGenerator.Model
{
	public class ExpressionContainer
	{
		public Expression Expression { get; set; }

		public string TargetObject { get; set; }

		public string TargetField { get; set; }

		public bool IsTargetingResource { get; set; }

		public string CommandParameterTarget { get; set; }

		public bool IsCommandParameterExpression { get; set; }
	}
}
