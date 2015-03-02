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

			// Generate property for ILocalizationService LocalizationService
			CodePropertyReferenceExpression localizationServiceReference = CreateLocalizationServiceProperty();

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

			List<ExpressionContainer> translationExpressions = expressions.Where(x => x.Expression.IsOfType(ExpressionType.Translation)).ToList();
			List<ExpressionContainer> expressionsTargetingResources = expressions.Where(x => x.IsTargetingResource && !x.Expression.IsOfType(ExpressionType.Translation)).ToList();
			List<ExpressionContainer> resourceExpressions = expressions.Where(x => !x.IsTargetingResource && x.Expression.IsOfType(ExpressionType.Resource)).ToList();
			List<ExpressionContainer> bindingExpressions = expressions.Where(x => !x.IsTargetingResource && x.Expression.IsOfType(ExpressionType.Binding)).ToList();

			CodeMethodReferenceExpression assignTranslationMethodReference = CreateAssignTranslationMethod(translationExpressions, localizationServiceReference);
			CodeMethodReferenceExpression setupResourcesReference = CreateSetupResourcesMethod(expressionsTargetingResources, resourceReferences);
			CodeMethodReferenceExpression setupResourceForViewElement = CreateSetupResourceForViewElementMethod(resourceExpressions, resourceReferences);
			CreateBindingOverrideMethod(bindingExpressions, localizationServiceReference, resourceReferences, assignTranslationMethodReference, setupResourcesReference, setupResourceForViewElement);
		}

		private CodePropertyReferenceExpression CreateLocalizationServiceProperty()
		{
			CodeMethodReferenceExpression resolveMethodReference = new CodeMethodReferenceExpression(
					new CodePropertyReferenceExpression(new CodeTypeReferenceExpression(CodeGeneratorHelper.GetTypeReferenceFromName("DependencyService")), "Container"),
					"Resolve",
					CodeGeneratorHelper.GetTypeReferenceFromName("ILocalizationService")
					);
			CodeMethodInvokeExpression invokeMethod = new CodeMethodInvokeExpression(resolveMethodReference);

			Tuple<CodeMemberField, CodeMemberProperty> result = CodeGeneratorHelper.GenerateProxyProperty(NameGeneratorHelper.LOCALIZATION_SERVICE_PROPERTY_NAME, "ILocalizationService", invokeMethod);

			Fields.Add(result.Item1);
			Properties.Add(result.Item2);

			return CodeGeneratorHelper.GetPropertyReference(result.Item2);
		}

		private void CreateBindingOverrideMethod(List<ExpressionContainer> bindingExpressions, CodePropertyReferenceExpression localizationServiceReference, Dictionary<string, CodePropertyReferenceExpression> resourceReferences, params CodeMethodReferenceExpression[] preCallMethods)
		{
			CodeMemberMethod method = new CodeMemberMethod
			{
				Attributes = MemberAttributes.Family | MemberAttributes.Override,
				Name = NameGeneratorHelper.GET_BINDING_METHOD_NAME,
				ReturnType = CodeGeneratorHelper.GetTypeReferenceFromName("List<BindingObject>"),
			};

			foreach (CodeMethodReferenceExpression preCallMethod in preCallMethods)
			{
				method.Statements.Add(new CodeMethodInvokeExpression(preCallMethod));
			}



			Methods.Add(method);
		}

		private CodeMethodReferenceExpression CreateAssignTranslationMethod(List<ExpressionContainer> expressions, CodePropertyReferenceExpression localizationServiceReference)
		{
			CodeMemberMethod method = new CodeMemberMethod
			{
				Attributes = MemberAttributes.Private,
				Name = NameGeneratorHelper.ASSIGN_TRANSLATION_METHOD_NAME,
			};

			foreach (ExpressionContainer expression in expressions)
			{
				List<CodeExpression> parameters = new List<CodeExpression>();
				if (expression.Expression.Has(TranslationExpression.UID))
				{
					//use method with two parameters.
					parameters.Add(new CodePrimitiveExpression(expression.Expression.GetValue(TranslationExpression.UID)));
				}
				if(expression.Expression.Has(TranslationExpression.KEY))
				{
					//use method with only key parameter.
					parameters.Add(new CodePrimitiveExpression(expression.Expression.GetValue(TranslationExpression.KEY)));
				}
				CodeMethodInvokeExpression valueReference = new CodeMethodInvokeExpression(localizationServiceReference, "GetString", parameters.ToArray());
				method.Statements.Add(SetValueStatement(CodeGeneratorHelper.GetPropertyReference(expression.TargetObject), expression.TargetField, valueReference))
			}

			Methods.Add(method);
			return CodeGeneratorHelper.GetMethodReference(method);
		}

		private CodeMethodReferenceExpression CreateSetupResourcesMethod(List<ExpressionContainer> expressions, Dictionary<string, CodePropertyReferenceExpression> resourceReferences)
		{
			CodeMemberMethod method = new CodeMemberMethod
			{
				Attributes = MemberAttributes.Private,
				Name = NameGeneratorHelper.ASSIGN_RESOURCE_TO_RESOURCE_METHOD_NAME,
			};

			// Create dependency graph to check for cycle in resource assignment
			Dictionary<string, DependencyNode> dependencies = resourceReferences.ToDictionary(x => x.Value.PropertyName, x => new DependencyNode(x.Value.PropertyName));
			foreach (ExpressionContainer expression in expressions)
			{
				string source = expression.TargetObject;
				string targetKey = expression.Expression.GetValue(ResourceExpression.KEY);
				string target = resourceReferences[targetKey].PropertyName;

				dependencies[source].Add(dependencies[target]);
			}

			// Check for cycle
			List<DependencyNode> processedNodes = new List<DependencyNode>();
			List<DependencyNode> waitingNodes = dependencies.Values.ToList();
			while (waitingNodes.Any())
			{
				DependencyNode node = waitingNodes.FirstOrDefault(x => x.Dependencies.All(o => o.IsMarked));

				if (node == null)
				{
					Log.LogError("Error : Circular references in Resources");
					throw new InvalidOperationException("Cirular references");
				}

				node.IsMarked = true;
				processedNodes.Add(node);
			}

			// If no cycles, we have the order to assign all resources in correct order
			Dictionary<DependencyNode, IEnumerable<ExpressionContainer>> orderedExpressions = processedNodes.ToDictionary(x => x, x => expressions.Where(o => o.TargetObject == x.Id));
			foreach (DependencyNode node in processedNodes)
			{
				foreach (ExpressionContainer expression in orderedExpressions[node])
				{
					string resourceKey = expression.Expression.GetValue(ResourceExpression.KEY);
					method.Statements.Add(SetValueStatement(CodeGeneratorHelper.GetPropertyReference(expression.TargetObject), expression.TargetField, resourceReferences[resourceKey]));
				}
			}

			Methods.Add(method);
			return CodeGeneratorHelper.GetMethodReference(method);
		}

		private CodeMethodReferenceExpression CreateSetupResourceForViewElementMethod(List<ExpressionContainer> expressions, Dictionary<string, CodePropertyReferenceExpression> resourceReferences)
		{
			CodeMemberMethod method = new CodeMemberMethod
			{
				Attributes = MemberAttributes.Private,
				Name = NameGeneratorHelper.ASSIGN_RESOURCE_TO_VIEW_METHOD_NAME,
			};

			foreach (ExpressionContainer expression in expressions)
			{
				string resourceKey = expression.Expression.GetValue(ResourceExpression.KEY);
				method.Statements.Add(SetValueStatement(CodeGeneratorHelper.GetPropertyReference(expression.TargetObject), expression.TargetField, resourceReferences[resourceKey]));
			}

			Methods.Add(method);
			return CodeGeneratorHelper.GetMethodReference(method);
		}

		private CodeStatement SetValueStatement(CodeExpression targetReference, string propertyTargetName, CodeExpression assignExpression)
		{
			if (Configuration.CaseSensitivity.GetValueOrDefault())
			{
				//In case CaseSensitivity is enabled, just give the value to the property
				return new CodeAssignStatement(new CodePropertyReferenceExpression(targetReference, propertyTargetName), assignExpression);
			}
			
			//If we are in mode CaseInsensitivity, use reflection to ignore case in property naming (slower execution)
			return CodeGeneratorHelper.GetSetValueWithReflectionStatement(targetReference, propertyTargetName, assignExpression);
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
