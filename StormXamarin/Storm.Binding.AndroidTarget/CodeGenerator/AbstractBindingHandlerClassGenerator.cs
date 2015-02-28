using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.CodeGenerator.Model;
using Storm.Binding.AndroidTarget.Compiler;
using Storm.Binding.AndroidTarget.Helper;
using Storm.Binding.AndroidTarget.Model;

namespace Storm.Binding.AndroidTarget.CodeGenerator
{
	public abstract class AbstractBindingHandlerClassGenerator : AbstractClassGenerator
	{
		private readonly BindingLanguageParser _expressionParser = new BindingLanguageParser();

		private TaskLoggingHelper Log { get { return BindingPreprocess.Logger; } }

		public void Preprocess(List<XmlAttribute> expressionAttributes, List<Resource> resources, List<IdViewObject> viewElements)
		{
			// Create all properties for viewElements
			Dictionary<string, CodePropertyReferenceExpression> viewElementReferences = viewElements.ToDictionary(x => x.Id, x =>
			{
				Tuple<CodeMemberField, CodeMemberProperty> result = CodeGeneratorHelper.GenerateProxyProperty(x.Id, x.TypeName, new CodeMethodInvokeExpression(GetFindViewByIdReference(x.TypeName), CodeGeneratorHelper.GetAndroidResourceReference(ResourcePart.Id, x.Id)));
				Fields.Add(result.Item1);
				Properties.Add(result.Item2);
				return CodeGeneratorHelper.GetPropertyReference(result.Item2);
			});

			// Eval all expressions
			List<ExpressionContainer> expressions = (from attribute in expressionAttributes
													let expressionResult = EvaluateExpression(attribute.Value)
													where expressionResult != null
													 select new ExpressionContainer
													{
														Expression = expressionResult,
														TargetObject = attribute.AttachedId,
														TargetField = attribute.LocalName,
														IsTargetingResource = false,
													}).ToList();
			// Affect a property name to all resources and check if some has expression as attribute value
			foreach (Resource res in resources)
			{
				res.PropertyName = NameGeneratorHelper.GetResourceName();
				foreach (KeyValuePair<string, string> propertyItem in res.Properties.Where(propertyItem => ParsingHelper.IsExpressionValue(propertyItem.Value)).ToList())
				{
					res.Properties.Remove(propertyItem.Key);

					Expression expr = EvaluateExpression(propertyItem.Value);
					if (expr != null)
					{
						if (CheckCorrectExpressionInResource(expr))
						{
							Log.LogError("Expression {0} is invalid in a resource context (you cannot use binding)", propertyItem.Value);
						}
						else
						{
							expressions.Add(new ExpressionContainer
							{
								Expression = expr,
								TargetObject = res.PropertyName,
								TargetField = propertyItem.Key,
								IsTargetingResource = true,
							});
						}
					}
				}
			}

			// Check if all resources are declared and filter those we need
			Dictionary<string, Resource> neededResource = new Dictionary<string, Resource>();
			List<string> resourceKeys = expressions.SelectMany(x => GetUsedResources(x.Expression)).Distinct().ToList();
			foreach (string key in resourceKeys)
			{
				Resource res = resources.FirstOrDefault(x => key.Equals(x.Key, StringComparison.InvariantCultureIgnoreCase));
				if (res == null)
				{
					Log.LogError("Resource with key {0} does not exists", key);
				}
				else
				{
					neededResource.Add(key, res);
				}
			}


			// Go through all binding expression and find those where we need to declare implicit resources
			// Will also remove all Template & TemplateSelector fields in BindingExpression
			// to only have a fully prepared adapter
			foreach (Expression bindingExpression in expressions.SelectMany(expression => GetBindingExpressions(expression.Expression)).ToList())
			{
				if (bindingExpression.Has(BindingExpression.TEMPLATE))
				{
					// create a template selector
					string templateSelectorKey = NameGeneratorHelper.GetResourceKey();
					string templateSelectorPropertyName = NameGeneratorHelper.GetResourceName();
					neededResource.Add(templateSelectorKey, new Resource(templateSelectorKey)
					{
						PropertyName = templateSelectorPropertyName,
						ResourceElement = null,
						Type = Configuration.DefaultTemplateSelector
					});
					expressions.Add(new ExpressionContainer
					{
						Expression = bindingExpression[BindingExpression.TEMPLATE],
						TargetField = Configuration.DefaultTemplateSelectorField,
						TargetObject = templateSelectorPropertyName,
						IsTargetingResource = true,
					});
					bindingExpression.Remove(BindingExpression.TEMPLATE);

					ResourceExpression templateSelectorResourceExpression = new ResourceExpression();
					templateSelectorResourceExpression.Add(ResourceExpression.KEY, new TextExpression
					{
						Value = templateSelectorKey
					});
					bindingExpression.Add(BindingExpression.TEMPLATE_SELECTOR, templateSelectorResourceExpression);
				}
				
				if (bindingExpression.Has(BindingExpression.TEMPLATE_SELECTOR))
				{
					// create an adapter
					string adapterKey = NameGeneratorHelper.GetResourceKey();
					string adapterName = NameGeneratorHelper.GetResourceName();
					neededResource.Add(adapterKey, new Resource(adapterKey)
					{
						PropertyName = adapterName,
						ResourceElement = null,
						Type = Configuration.DefaultAdapter
					});
					expressions.Add(new ExpressionContainer()
					{
						Expression = bindingExpression[BindingExpression.TEMPLATE_SELECTOR],
						TargetField = Configuration.DefaultAdapterField,
						TargetObject = adapterName,
						IsTargetingResource = true,
					});
					bindingExpression.Remove(BindingExpression.TEMPLATE_SELECTOR);
					ResourceExpression adapterResourceExpression = new ResourceExpression();
					adapterResourceExpression.Add(ResourceExpression.KEY, new TextExpression
					{
						Value = adapterKey
					});
					bindingExpression.Add(BindingExpression.ADAPTER, adapterResourceExpression);
				}
			}

			// Create all properties for resources
			Dictionary<string, CodePropertyReferenceExpression> resourceReferences = CreatePropertiesForResources(neededResource.Values);


			// Create a setup resources method to initalize resources with all {Resource ...} and {Translation ...} expressions
			List<ExpressionContainer> expressionsTargetingResources = expressions.Where(x => x.IsTargetingResource).ToList();
			expressions.RemoveAll(x => x.IsTargetingResource);

			CodeMethodReferenceExpression setupResourcesReference = CreateSetupResourcesMethod(expressionsTargetingResources, resourceReferences);

		}

		private CodeMethodReferenceExpression CreateSetupResourcesMethod(List<ExpressionContainer> expressions, Dictionary<string, CodePropertyReferenceExpression> resourceReferences)
		{
			List<ExpressionContainer> translationExpressions = expressions.Where(x => x.Expression.IsOfType(ExpressionType.Translation)).ToList();
			List<ExpressionContainer> resourceExpressions = expressions.Where(x => x.Expression.IsOfType(ExpressionType.Resource)).ToList();

			CodeMemberMethod method = new CodeMemberMethod()
			{
				Attributes = MemberAttributes.Private,
				Name = NameGeneratorHelper.SETUP_RESOURCES_NAME,
			};

			// TODO : rework it => should have a 
			//		=> Translation setup (for all top level translation)
			//		=> Resource setup (for all top level resource)
			//		=> Binding (for returning)


			method.Statements.Add(CodeGeneratorHelper.CreateStartRegionStatement("Translation setup"));
			method.Statements.AddRange(CreateStatementsForTranslation(translationExpressions).ToArray());
			method.Statements.Add(CodeGeneratorHelper.CreateEndRegionStatement());

			method.Statements.Add(CodeGeneratorHelper.CreateStartRegionStatement("Resource setup"));
			method.Statements.AddRange(CreateStatementsForTranslation(translationExpressions).ToArray());
			method.Statements.Add(CodeGeneratorHelper.CreateEndRegionStatement());

			Methods.Add(method);

			return CodeGeneratorHelper.GetMethodReference(method);
		}

		private List<CodeStatement> CreateStatementsForTranslation(List<ExpressionContainer> translationExpressions)
		{
			List<CodeStatement> statements = new List<CodeStatement>();

			foreach (ExpressionContainer expression in translationExpressions)
			{
				
			}

			return statements;
		}

		private Dictionary<string, CodePropertyReferenceExpression> CreatePropertiesForResources(IEnumerable<Resource> resources)
		{
			return resources.ToDictionary(resource => resource.Key, resource =>
			{
				ResourceWithId resourceWithId = resource as ResourceWithId;
				if (resourceWithId != null) // in case of data templates
				{
					// Just return the id
					// int MyResource { get { return Resource.Id.****; } }
					List<CodeStatement> statements = new List<CodeStatement>
					{
						new CodeMethodReturnStatement(CodeGeneratorHelper.GetAndroidResourceReference(ResourcePart.Layout, resourceWithId.ResourceId)),
					};
					CodeMemberProperty propertyResult = CodeGeneratorHelper.GenerateProperty(resource.PropertyName, "int", statements);
					Properties.Add(propertyResult);
					return CodeGeneratorHelper.GetPropertyReference(propertyResult);
				}

				// create a proxy property to handle the resource
				string type = resource.Type;
				Dictionary<string, string> assignments = resource.Properties;
				Tuple<CodeMemberField, CodeMemberProperty> result = CodeGeneratorHelper.GenerateProxyProperty(resource.PropertyName, type, fieldReference => CodeGeneratorHelper.GenerateStatementsCreateAndAssign(fieldReference, type, assignments));

				Fields.Add(result.Item1);
				Properties.Add(result.Item2);
				return CodeGeneratorHelper.GetPropertyReference(result.Item2);
			});
		}

		private List<Expression> GetBindingExpressions(Expression expression)
		{
			List<Expression> result = new List<Expression>();

			if (expression.IsOfType(ExpressionType.Binding))
			{
				result.Add(expression);
			}

			result.AddRange(expression.Attributes.Values.SelectMany(GetBindingExpressions));
			
			return result;
		}

		private Expression EvaluateExpression(string content)
		{
			bool result;
			Expression expression = _expressionParser.Parse(content, out result);

			if (!result)
			{
				Log.LogError("Syntax error in Binding expression '{0}'", content);
				return null;
			}

			if (!expression.CheckCorrectness())
			{
				Log.LogError("Invalid binding expression '{0}'", content);
				return null;
			}
			return expression;
		}

		private bool CheckCorrectExpressionInResource(Expression expr)
		{
			if (expr.IsOfType(ExpressionType.Binding))
			{
				return false;
			}

			foreach (Expression child in expr.Attributes.Values)
			{
				if (!CheckCorrectExpressionInResource(child))
				{
					return false;
				}
			}
			return true;
		}

		private List<string> GetUsedResources(Expression expression)
		{
			List<string> result = new List<string>();

			if (expression.IsOfType(ExpressionType.Resource))
			{
				result.Add(expression.GetValue(ResourceExpression.KEY));
			}
			foreach (Expression child in expression.Attributes.Values)
			{
				result.AddRange(GetUsedResources(child));
			}

			return result;
		}

		protected abstract CodeMethodReferenceExpression GetFindViewByIdReference(string typeName);

		protected abstract CodePropertyReferenceExpression GetLayoutInflaterReference();
	}
}
