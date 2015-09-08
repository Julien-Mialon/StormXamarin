using System.CodeDom;
using Storm.MvvmCross.Android.Target.Helper;

namespace Storm.MvvmCross.Android.Target.CodeGenerator
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
