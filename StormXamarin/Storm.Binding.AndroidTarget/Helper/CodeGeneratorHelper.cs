using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace Storm.Binding.AndroidTarget.Helper
{
	public static class CodeGeneratorHelper
	{
		private static Dictionary<string, Type> _baseTypes = new Dictionary<string, Type>
		{
			{"int", typeof(int)},
			{"bool", typeof(bool)},
			{"short", typeof(short)},
			{"long", typeof(long)},
			{"float", typeof(float)},
			{"double", typeof(double)},
			{"string", typeof(string)},
			{"char", typeof(char)},
		};

		public static CodeTypeReference GetTypeReferenceFromName(string typeName)
		{
			if (_baseTypes.ContainsKey(typeName))
			{
				return new CodeTypeReference(_baseTypes[typeName]);
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
	}
}
