using System.CodeDom;

namespace Storm.Binding.AndroidTarget.Process
{
	public class ViewHolderClassGenerator : AbstractClassGenerator
	{
		public ViewHolderClassGenerator(string namespaceName, string className) : base(namespaceName, className, new CodeTypeReference("BaseViewHolder"))
		{
			IsPartialClass = false;
		}

		protected override void GenerateOtherMembers(CodeTypeDeclaration classDeclaration)
		{
			base.GenerateOtherMembers(classDeclaration);

			CodeConstructor constructor = new CodeConstructor
			{
				Attributes = MemberAttributes.Public
			};
			constructor.Parameters.Add(new CodeParameterDeclarationExpression("LayoutInflater", "layoutInflater"));
			constructor.Parameters.Add(new CodeParameterDeclarationExpression("View", "view"));

			constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("layoutInflater"));
			constructor.BaseConstructorArgs.Add(new CodeVariableReferenceExpression("view"));

			classDeclaration.Members.Add(constructor);
		}

		protected override CodeMethodReferenceExpression GetFindViewReferenceExpression(string type)
		{
			return new CodeMethodReferenceExpression(
						new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "View"),
						"FindViewById",
						new CodeTypeReference(type)
						);
		}

		protected override CodeExpression GetLayoutInflaterReferenceExpression()
		{
			return new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "LayoutInflater");
		}

		protected override string GetOverrideMethodName()
		{
			return "GetBindingPaths";
		}

		protected override MemberAttributes GetOverrideMethodVisibility()
		{
			return MemberAttributes.Public;
		}
	}
}

