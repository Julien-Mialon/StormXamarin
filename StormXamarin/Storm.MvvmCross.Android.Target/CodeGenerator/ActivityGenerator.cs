using System.CodeDom;
using Storm.MvvmCross.Android.Target.Helper;

namespace Storm.MvvmCross.Android.Target.CodeGenerator
{
	class ActivityGenerator : AbstractBindingHandlerClassGenerator
	{
		protected override CodeMethodReferenceExpression GetFindViewByIdReference(string typeName)
		{
			return new CodeMethodReferenceExpression(
						new CodeThisReferenceExpression(),
						"FindViewById",
						CodeGeneratorHelper.GetTypeReferenceFromName(typeName));
		}

		protected override CodePropertyReferenceExpression GetLayoutInflaterReference()
		{
			return CodeGeneratorHelper.GetPropertyReference("LayoutInflater");
		}
	}
}
