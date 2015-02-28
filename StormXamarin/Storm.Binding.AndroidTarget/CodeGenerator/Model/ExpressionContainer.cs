using Storm.Binding.AndroidTarget.Compiler;

namespace Storm.Binding.AndroidTarget.CodeGenerator.Model
{
	public class ExpressionContainer
	{
		public Expression Expression { get; set; }

		public string TargetObject { get; set; }

		public string TargetField { get; set; }

		public bool IsTargetingResource { get; set; }
	}
}
