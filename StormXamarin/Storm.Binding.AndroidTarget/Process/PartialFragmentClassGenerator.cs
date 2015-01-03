using System.CodeDom;

namespace Storm.Binding.AndroidTarget.Process
{
	public class PartialFragmentClassGenerator : AbstractClassGenerator
	{
		public PartialFragmentClassGenerator(string namespaceName, string className) : base(namespaceName, className, null)
		{
			IsPartialClass = true;
		}

		protected override CodeMethodReferenceExpression GetFindViewReferenceExpression(string type)
		{
			return new CodeMethodReferenceExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "RootView"),
						"FindViewById",
						new CodeTypeReference(type)
						);
		}

		protected override CodeExpression GetLayoutInflaterReferenceExpression()
		{
			return new CodePropertyReferenceExpression(new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "Activity"), "LayoutInflater");
		}

		protected override string GetOverrideMethodName()
		{
			return "GetBindingPaths";
		}

		protected override MemberAttributes GetOverrideMethodVisibility()
		{
			return MemberAttributes.Family;
		}
	}
}
