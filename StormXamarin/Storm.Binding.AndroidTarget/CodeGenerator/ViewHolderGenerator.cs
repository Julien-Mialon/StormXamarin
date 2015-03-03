using System.CodeDom;
using Storm.Binding.AndroidTarget.Helper;

namespace Storm.Binding.AndroidTarget.CodeGenerator
{
	class ViewHolderGenerator : AbstractBindingHandlerClassGenerator
	{
		protected override CodeMethodReferenceExpression GetFindViewByIdReference(string typeName)
		{
			return new CodeMethodReferenceExpression(
						CodeGeneratorHelper.GetPropertyReference("View"),
						"FindViewById",
						CodeGeneratorHelper.GetTypeReferenceFromName(typeName)
						);
		}

		protected override CodePropertyReferenceExpression GetLayoutInflaterReference()
		{
			return CodeGeneratorHelper.GetPropertyReference("LayoutInflater");
		}
	}
}
