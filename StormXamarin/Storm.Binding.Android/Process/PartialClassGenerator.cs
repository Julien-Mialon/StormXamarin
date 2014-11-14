using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Storm.Binding.Android.Data;
using System.CodeDom;

namespace Storm.Binding.Android.Process
{
	class PartialClassGenerator
	{
		public void Generate(ActivityInfo activityInformations, List<XmlAttribute> bindingInformations)
		{
			CodeCompileUnit codeUnit = new CodeCompileUnit();
			CodeNamespace codeNamespace = new CodeNamespace(activityInformations.NamespaceName);
			codeUnit.Namespaces.Add(codeNamespace);

			CodeTypeDeclaration classDeclaration = new CodeTypeDeclaration(activityInformations.ClassName)
			{
				IsClass = true,
				IsPartial = true,
				TypeAttributes = TypeAttributes.Public,
			};

			codeNamespace.Types.Add(classDeclaration);
			codeNamespace.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
			codeNamespace.Imports.Add(new CodeNamespaceImport("Storm.Mvvm.Android.Bindings"));

			CodeMemberMethod overrideMethod = new CodeMemberMethod
			{
				Attributes = MemberAttributes.Family | MemberAttributes.Override,
				Name = "GetBindingPaths",
				ReturnType = new CodeTypeReference("List<BindingObject>")
			};

			GenerateMethodContent(overrideMethod, bindingInformations);
			classDeclaration.Members.Add(overrideMethod);


			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			CodeGeneratorOptions options = new CodeGeneratorOptions
			{
				BlankLinesBetweenMembers = true,
				BracingStyle = "C",
				IndentString = "\t"
			};
			using (StreamWriter writer = new StreamWriter(activityInformations.OutputFile))
			{
				provider.GenerateCodeFromCompileUnit(codeUnit, writer, options);
			}
		}

		private void GenerateMethodContent(CodeMemberMethod method, List<XmlAttribute> bindings)
		{
			CodeObjectCreateExpression resultCollectionInitializer = new CodeObjectCreateExpression("List<BindingObject>");
			method.Statements.Add(new CodeVariableDeclarationStatement("List<BindingObject>", "result", resultCollectionInitializer));

			CodeVariableReferenceExpression resultReference = new CodeVariableReferenceExpression("result");

			int objectCounter = 0;
			int expressionCounter = 0;

			List<BindingExpression> expressions = ParseBindings(bindings);
			// group by binding objects
			foreach (IGrouping<string, BindingExpression> bindingExpressions in expressions.GroupBy(x => x.TargetObjectId))
			{
				//create binding objects and get a code reference on it
				CodeObjectCreateExpression objectCreateExpression = new CodeObjectCreateExpression("BindingObject", new CodePrimitiveExpression(bindingExpressions.Key));
				string objectName = string.Format("o{0}", objectCounter++);
				method.Statements.Add(new CodeVariableDeclarationStatement("BindingObject", objectName, objectCreateExpression));

				CodeVariableReferenceExpression objectReference = new CodeVariableReferenceExpression(objectName);

				//add the object to the result list
				method.Statements.Add(new CodeMethodInvokeExpression(resultReference, "Add", objectReference));

				//add all expressions
				foreach (BindingExpression expr in expressions)
				{
					CodeObjectCreateExpression exprCreateExpression = new CodeObjectCreateExpression("BindingExpression", new CodePrimitiveExpression(expr.TargetFieldId), new CodePrimitiveExpression(expr.SourcePath));
					string exprName = string.Format("e{0}", expressionCounter++);
					method.Statements.Add(new CodeVariableDeclarationStatement("BindingExpression", exprName, exprCreateExpression));

					CodeVariableReferenceExpression exprReference = new CodeVariableReferenceExpression(exprName);
					method.Statements.Add(new CodeMethodInvokeExpression(objectReference, "AddExpression", exprReference));
				}
			}


			method.Statements.Add(new CodeMethodReturnStatement(resultReference));
		}

		private List<BindingExpression> ParseBindings(IEnumerable<XmlAttribute> bindings)
		{
			const string BINDING_EXPRESSION_START = "{Binding";
			const string BINDING_EXPRESSION_END = "}";
			List<BindingExpression> result = new List<BindingExpression>();

			foreach (XmlAttribute attribute in bindings)
			{
				string bindingValue = attribute.Value;
				if (bindingValue.StartsWith(BINDING_EXPRESSION_START) && bindingValue.EndsWith(BINDING_EXPRESSION_END))
				{
					bindingValue = bindingValue.Substring(BINDING_EXPRESSION_START.Length);
					bindingValue = bindingValue.Substring(0, bindingValue.Length - 1).Trim();

					BindingExpression expr = new BindingExpression()
					{
						TargetFieldId = attribute.Name,
						TargetObjectId = attribute.AttachedId,
						SourcePath = bindingValue,
					};

					result.Add(expr);
				}
				else
				{
					Console.WriteLine("Error on : " + bindingValue);
				}
			}

			return result;
		}

		class BindingExpression
		{
			public string TargetObjectId { get; set; }

			public string TargetFieldId { get; set; }

			public string SourcePath { get; set; }
		}
	}
}
