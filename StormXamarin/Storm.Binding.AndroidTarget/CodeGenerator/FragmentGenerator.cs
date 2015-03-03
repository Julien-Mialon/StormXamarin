using System.CodeDom;
using Storm.Binding.AndroidTarget.Helper;

namespace Storm.Binding.AndroidTarget.CodeGenerator
{
	class FragmentGenerator : AbstractBindingHandlerClassGenerator
	{
		protected override CodeMethodReferenceExpression GetFindViewByIdReference(string typeName)
		{
			return new CodeMethodReferenceExpression(
						CodeGeneratorHelper.GetPropertyReference("RootView"),
						"FindViewById",
						CodeGeneratorHelper.GetTypeReferenceFromName(typeName)
						);
		}

		protected override CodePropertyReferenceExpression GetLayoutInflaterReference()
		{
			return new CodePropertyReferenceExpression(CodeGeneratorHelper.GetPropertyReference("Activity"), "LayoutInflater");
		}
	}
}
