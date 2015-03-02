using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Storm.Binding.AndroidTarget.Model;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace Storm.Binding.AndroidTarget.Helper
{
	public static class CodeGeneratorHelper
	{
		#region base types

		private static readonly Dictionary<string, Type> BaseTypes = new Dictionary<string, Type>
		{
			{"int", typeof (int)},
			{"bool", typeof (bool)},
			{"short", typeof (short)},
			{"long", typeof (long)},
			{"float", typeof (float)},
			{"double", typeof (double)},
			{"string", typeof (string)},
			{"char", typeof (char)},
		};

		#endregion

		public static CodeTypeReference GetTypeReferenceFromName(string typeName)
		{
			if (BaseTypes.ContainsKey(typeName))
			{
				return new CodeTypeReference(BaseTypes[typeName]);
			}
			Type type = Type.GetType(typeName, false);
			if (type == null || type.FullName.StartsWith("Storm.Binding.AndroidTarget"))
			{
				return new CodeTypeReference(typeName);
			}
			return new CodeTypeReference(type);
		}

		public static CodeMemberProperty GenerateProperty(string propertyName, string propertyType, List<CodeStatement> getStatements = null, List<CodeStatement> setStatements = null)
		{
			CodeMemberProperty property = new CodeMemberProperty
			{

				Attributes = MemberAttributes.Family | MemberAttributes.Final,
				Name = propertyName,
				Type = GetTypeReferenceFromName(propertyType),
				HasGet = (getStatements != null && getStatements.Any()),
				HasSet = (setStatements != null && setStatements.Any())
			};

			if (property.HasGet && getStatements != null)
			{
				foreach (CodeStatement statement in getStatements)
				{
					property.GetStatements.Add(statement);
				}
			}

			if (property.HasSet && setStatements != null)
			{
				foreach (CodeStatement statement in setStatements)
				{
					property.SetStatements.Add(statement);
				}
			}

			return property;
		}

		public static CodeMemberField GenerateField(string fieldType)
		{
			string fieldName = NameGeneratorHelper.GetFieldName();
			CodeMemberField field = new CodeMemberField(GetTypeReferenceFromName(fieldType), fieldName)
			{
				Attributes = MemberAttributes.Private
			};

			return field;
		}

		public static Tuple<CodeVariableDeclarationStatement, CodeVariableReferenceExpression> CreateVariable(CodeTypeReference typeReference, string name, params CodeExpression[] constructorParameters)
		{
			CodeObjectCreateExpression initializer = new CodeObjectCreateExpression(typeReference, constructorParameters);
			CodeVariableDeclarationStatement statement = new CodeVariableDeclarationStatement(typeReference, name, initializer);
			CodeVariableReferenceExpression reference = new CodeVariableReferenceExpression(name);

			return new Tuple<CodeVariableDeclarationStatement, CodeVariableReferenceExpression>(statement, reference);
		}

		public static CodePropertyReferenceExpression GetPropertyReference(string propertyName)
		{
			return new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), propertyName);
		}

		public static CodePropertyReferenceExpression GetPropertyReference(CodeMemberProperty property)
		{
			return GetPropertyReference(property.Name);
		}
		
		public static CodeFieldReferenceExpression GetFieldReference(string fieldName)
		{
			return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
		}

		public static CodeFieldReferenceExpression GetFieldReference(CodeMemberField field)
		{
			return GetFieldReference(field.Name);
		}

		public static CodeMethodReferenceExpression GetMethodReference(string methodName)
		{
			return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), methodName);
		}

		public static CodeMethodReferenceExpression GetMethodReference(CodeMemberMethod method)
		{
			return new CodeMethodReferenceExpression(new CodeThisReferenceExpression(), method.Name);
		}

		public static Tuple<CodeMemberField, CodeMemberProperty> GenerateProxyProperty(string propertyName, string propertyType, CodeExpression rightAssignExpression)
		{
			CodeMemberField field = GenerateField(propertyType);
			CodeFieldReferenceExpression fieldReference = GetFieldReference(field);

			List<CodeStatement> getStatements = new List<CodeStatement>();

			CodeConditionStatement ifStatement = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(fieldReference, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null)),
				new CodeAssignStatement(fieldReference, rightAssignExpression)
			);

			getStatements.Add(ifStatement);
			getStatements.Add(new CodeMethodReturnStatement(fieldReference));

			CodeMemberProperty property = GenerateProperty(propertyName, propertyType, getStatements);

			return new Tuple<CodeMemberField, CodeMemberProperty>(field, property);
		}

		public static Tuple<CodeMemberField, CodeMemberProperty> GenerateProxyProperty(string propertyName, string propertyType, Func<CodeFieldReferenceExpression, List<CodeStatement>> assignStatementsGenerator)
		{
			CodeMemberField field = GenerateField(propertyType);
			CodeFieldReferenceExpression fieldReference = GetFieldReference(field);

			List<CodeStatement> getStatements = new List<CodeStatement>();

			CodeConditionStatement ifStatement = new CodeConditionStatement(
				new CodeBinaryOperatorExpression(fieldReference, CodeBinaryOperatorType.IdentityEquality, new CodePrimitiveExpression(null)),
				assignStatementsGenerator(fieldReference).ToArray()
			);

			getStatements.Add(ifStatement);
			getStatements.Add(new CodeMethodReturnStatement(fieldReference));

			CodeMemberProperty property = GenerateProperty(propertyName, propertyType, getStatements);

			return new Tuple<CodeMemberField, CodeMemberProperty>(field, property);
		}

		public static CodeFieldReferenceExpression GetAndroidResourceReference(ResourcePart resourcePart, string name)
		{
			string str;
			switch (resourcePart)
			{
				case ResourcePart.Id:
					str = "Id";
					break;
				case ResourcePart.Layout:
					str = "Layout";
					break;
				default:
					throw new ArgumentOutOfRangeException("resourcePart", resourcePart, "Expected Id or Layout");
			}

			return new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(string.Format("Resource.{0}", str)), name);
		}

		public static List<CodeStatement> GenerateStatementsCreateAndAssign(CodeFieldReferenceExpression fieldReference, string type, Dictionary<string, string> propertyAssignments)
		{
			List<CodeStatement> statements = new List<CodeStatement>
			{
				new CodeAssignStatement(fieldReference, new CodeObjectCreateExpression(GetTypeReferenceFromName(type)))
			};
			statements.AddRange(propertyAssignments.Select(item => 
				new CodeAssignStatement(new CodePropertyReferenceExpression(fieldReference, item.Key), new CodePrimitiveExpression(item.Value))));

			return statements;
		}

		public static CodeStatement CreateStartRegionStatement(string name)
		{
			CodeStatement statement = new CodeStatement();
			statement.StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, name));
			return statement;
		}

		public static CodeStatement CreateEndRegionStatement()
		{
			CodeStatement statement = new CodeStatement();
			statement.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, ""));
			return statement;
		}

		public static CodeStatement GetSetValueWithReflectionStatement(CodeExpression targetObjectReference, string targetField, CodeExpression value)
		{
			CodeMethodInvokeExpression getTypeMethodInvoke = new CodeMethodInvokeExpression(targetObjectReference, "GetType");
			CodeMethodInvokeExpression getPropertyMethodInvoke = new CodeMethodInvokeExpression(getTypeMethodInvoke, "GetProperty",
				new CodePrimitiveExpression(targetField),
				new CodeSnippetExpression("BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance")
			);
			CodeMethodInvokeExpression setValueMethodInvoke = new CodeMethodInvokeExpression(getPropertyMethodInvoke, "SetValue", targetObjectReference, value);
			return new CodeExpressionStatement(setValueMethodInvoke);
		}
	}
}
