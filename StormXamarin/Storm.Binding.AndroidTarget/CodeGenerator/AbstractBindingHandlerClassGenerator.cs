using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.CodeGenerator.Model;
using Storm.Binding.AndroidTarget.Compiler;
using Storm.Binding.AndroidTarget.Helper;
using Storm.Binding.AndroidTarget.Model;

// ReSharper disable BitwiseOperatorOnEnumWithoutFlags

namespace Storm.Binding.AndroidTarget.CodeGenerator
{
	public abstract class AbstractBindingHandlerClassGenerator : AbstractClassGenerator
	{
		private readonly BindingLanguageParser _expressionParser = new BindingLanguageParser();

		private TaskLoggingHelper Log { get { return BindingPreprocess.Logger; } }

		public void Preprocess(List<XmlAttribute> expressionAttributes, List<Resource> resources, List<IdViewObject> viewElements)
		{
			// Create all properties for viewElements
			foreach (IdViewObject viewElement in viewElements)
			{
				Tuple<CodeMemberField, CodeMemberProperty> result = CodeGeneratorHelper.GenerateProxyProperty(viewElement.Id, viewElement.TypeName, new CodeMethodInvokeExpression(GetFindViewByIdReference(viewElement.TypeName), CodeGeneratorHelper.GetAndroidResourceReference(ResourcePart.Id, viewElement.Id)));
				Fields.Add(result.Item1);
				Properties.Add(result.Item2);
			}

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
					expressions.Add(new ExpressionContainer
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

			// In order to check if all adapter are not used more than once since we need them to be unique target
			Dictionary<string, bool> usedAdapter = new Dictionary<string, bool>();
			foreach(ExpressionContainer expression in expressions.Where(x => x.Expression.IsOfType(ExpressionType.Binding)).ToList())
			{
				Expression bindingExpression = expression.Expression;
				if (bindingExpression.Has(BindingExpression.ADAPTER))
				{
					// expression in Adapter could only be Resource (since it's an android platform specific things, a binding expression would not have any sense)
					Expression resourceExpression = bindingExpression[BindingExpression.ADAPTER];
					string adapterKey = resourceExpression.GetValue(ResourceExpression.KEY);
					Resource adapterResource = neededResource[adapterKey];

					if (usedAdapter.ContainsKey(adapterKey))
					{
						Log.LogError("The adapter with key {0} is used more than once which could lead to issue, you need one adapter per use !", adapterKey);
					}
					else
					{
						usedAdapter.Add(adapterKey, true);
					}

					// remove the adapter property
					bindingExpression.Remove(BindingExpression.ADAPTER);

					// store old target info
					string oldTargetField = expression.TargetField;
					string oldTargetObject = expression.TargetObject;
					bool oldTargetType = expression.IsTargetingResource;

					// retarget the binding expression to be targeted to Adapter.Collection
					expression.TargetField = "Collection";
					expression.TargetObject = adapterResource.PropertyName;
					expression.IsTargetingResource = false; //TODO : false for debug mode only, need to see what we can do about that ?

					// add a new expression to target the old object/field couple and affect the adapter with the resource expression
					expressions.Add(new ExpressionContainer
					{
						IsTargetingResource = oldTargetType,
						TargetField = oldTargetField,
						TargetObject = oldTargetObject,
						Expression = resourceExpression,
					});
				}
			}

			// Create all properties for resources
			Dictionary<string, CodePropertyReferenceExpression> resourceReferences = CreatePropertiesForResources(neededResource.Values);
			// Generate all properties to handle CommandParameter and retarget all expressions if needed
			GenerateCommandParameterProperties(expressions);

			// Create a setup resources method to initalize resources with all {Resource ...} and {Translation ...} expressions
			List<ExpressionContainer> translationExpressions = expressions.Where(x => x.Expression.IsOfType(ExpressionType.Translation)).ToList();
			List<ExpressionContainer> expressionsTargetingResources = expressions.Where(x => x.IsTargetingResource && x.Expression.IsOfType(ExpressionType.Resource)).ToList();
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
			CodeTypeReference listOfBindingObjectTypeReference = CodeGeneratorHelper.GetTypeReferenceFromName("List<BindingObject>");
			CodeTypeReference bindingObjectTypeReference = CodeGeneratorHelper.GetTypeReferenceFromName("BindingObject");
			CodeTypeReference bindingExpressionTypeReference = CodeGeneratorHelper.GetTypeReferenceFromName("BindingExpression");

			CodeMemberMethod method = new CodeMemberMethod
			{
				Attributes = MemberAttributes.Family | MemberAttributes.Override,
				Name = NameGeneratorHelper.GET_BINDING_METHOD_NAME,
				ReturnType = listOfBindingObjectTypeReference
			};

			foreach (CodeMethodReferenceExpression preCallMethod in preCallMethods)
			{
				method.Statements.Add(new CodeMethodInvokeExpression(preCallMethod));
			}

			var variableCreationResult = CodeGeneratorHelper.CreateVariable(listOfBindingObjectTypeReference, "result");
			method.Statements.Add(variableCreationResult.Item1);
			CodeVariableReferenceExpression resultReference = variableCreationResult.Item2;

			// group binding expression by target object to simplify
			foreach (IGrouping<string, ExpressionContainer> groupedExpressions in bindingExpressions.GroupBy(x => x.TargetObject))
			{
				// create a variable for this binding object and build it with the Property of the view element
				variableCreationResult = CodeGeneratorHelper.CreateVariable(bindingObjectTypeReference, NameGeneratorHelper.GetBindingObjectName(), CodeGeneratorHelper.GetPropertyReference(groupedExpressions.Key));//viewElementReferences[groupedExpressions.Key]);
				method.Statements.Add(variableCreationResult.Item1);

				CodeVariableReferenceExpression objectReference = variableCreationResult.Item2;
				method.Statements.Add(new CodeMethodInvokeExpression(resultReference, "Add", objectReference));


				foreach (ExpressionContainer expressionContainer in groupedExpressions)
				{
					Expression expression = expressionContainer.Expression;
					// create a binding expression for this
					variableCreationResult = CodeGeneratorHelper.CreateVariable(bindingExpressionTypeReference, NameGeneratorHelper.GetBindingExpressionName(), new CodePrimitiveExpression(expressionContainer.TargetField),
						new CodePrimitiveExpression(expression.GetValue(BindingExpression.PATH)));
					method.Statements.Add(variableCreationResult.Item1);

					CodeVariableReferenceExpression expressionReference = variableCreationResult.Item2;

					if (expression.Has(BindingExpression.MODE) && expression.Get<ModeExpression>(BindingExpression.MODE).Value != BindingMode.OneWay)
					{
						// Expression could only be of type ModeExpression
						BindingMode mode = expression.Get<ModeExpression>(BindingExpression.MODE).Value;
						method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "Mode"),
							new CodeFieldReferenceExpression(new CodeTypeReferenceExpression("BindingMode"), mode.ToString())));

					}
					if (expression.Has(BindingExpression.UPDATE_EVENT) && !string.IsNullOrWhiteSpace(expression.GetValue(BindingExpression.UPDATE_EVENT)))
					{
						// Expression could only be of type Text
						string updateEvent = expression.GetValue(BindingExpression.UPDATE_EVENT);
						method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "UpdateEvent"), new CodePrimitiveExpression(updateEvent)));
					}
					if (expression.Has(BindingExpression.CONVERTER))
					{
						// Expression could be type Resource or (Binding : not implemented for now)
						Expression converterExpression = expression[BindingExpression.CONVERTER];
						if (converterExpression.IsOfType(ExpressionType.Binding))
						{
							Log.LogError("Binding expression for converter is not implemented yet");
							throw new NotImplementedException("Binding expression for converter is not implemented yet");
						}
						else if (converterExpression.IsOfType(ExpressionType.Resource))
						{
							string resourceKey = converterExpression.GetValue(ResourceExpression.KEY);
							CodePropertyReferenceExpression resourceReference = resourceReferences[resourceKey];

							method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "Converter"), resourceReference));
						}
					}
					if (expression.Has(BindingExpression.CONVERTER_PARAMETER))
					{
						// Expression could be of type Resource, Translation, Text or (Binding : not implemented for now)
						Expression converterParameterExpression = expression[BindingExpression.CONVERTER_PARAMETER];
						if (converterParameterExpression.IsOfType(ExpressionType.Binding))
						{
							Log.LogError("Binding expression for converter parameter is not implemented yet");
							throw new NotImplementedException("Binding expression for converter parameter is not implemented yet");
						}
						else if (converterParameterExpression.IsOfType(ExpressionType.Resource))
						{
							string resourceKey = converterParameterExpression.GetValue(ResourceExpression.KEY);
							CodePropertyReferenceExpression resourceReference = resourceReferences[resourceKey];

							method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "ConverterParameter"), resourceReference));
						}
						else if (converterParameterExpression.IsOfType(ExpressionType.Translation))
						{
							CodeMethodInvokeExpression valueReference = GenerateStatementToGetTranslation(localizationServiceReference, converterParameterExpression);
							method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "ConverterParameter"), valueReference));
						}
						else if (converterParameterExpression.IsOfType(ExpressionType.Value))
						{
							TextExpression textExpression = converterParameterExpression as TextExpression;
							if (textExpression != null)
							{
								string converterParameterValue = textExpression.Value;
								method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "ConverterParameter"), new CodePrimitiveExpression(converterParameterValue)));
							}
							else
							{
								throw new InvalidOperationException("Can't get a textExpression from a ExpressionType.Value expression...");
							}
						}
					}

					if (!string.IsNullOrWhiteSpace(expressionContainer.CommandParameterTarget))
					{
						method.Statements.Add(new CodeAssignStatement(new CodePropertyReferenceExpression(expressionReference, "CommandParameter"),
							CodeGeneratorHelper.GetPropertyReference(expressionContainer.CommandParameterTarget)));
					}

					method.Statements.Add(new CodeMethodInvokeExpression(objectReference, "AddExpression", expressionReference));
				}
			}

			method.Statements.Add(new CodeMethodReturnStatement(resultReference));

			Methods.Add(method);
		}

		private CodeMethodInvokeExpression GenerateStatementToGetTranslation(CodePropertyReferenceExpression localizationServiceReference, Expression translationExpression)
		{
			List<CodeExpression> parameters = new List<CodeExpression>();
			if (translationExpression.Has(TranslationExpression.UID))
			{
				//use method with two parameters.
				parameters.Add(new CodePrimitiveExpression(translationExpression.GetValue(TranslationExpression.UID)));
			}
			if (translationExpression.Has(TranslationExpression.KEY))
			{
				//use method with only key parameter.
				parameters.Add(new CodePrimitiveExpression(translationExpression.GetValue(TranslationExpression.KEY)));
			}
			CodeMethodInvokeExpression valueReference = new CodeMethodInvokeExpression(localizationServiceReference, "GetString", parameters.ToArray());
			return valueReference;
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
				CodeMethodInvokeExpression valueReference = GenerateStatementToGetTranslation(localizationServiceReference, expression.Expression);
				method.Statements.Add(SetValueStatement(CodeGeneratorHelper.GetPropertyReference(expression.TargetObject), expression.TargetField, valueReference));
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
				waitingNodes.Remove(node);
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

		private void GenerateCommandParameterProperties(List<ExpressionContainer> expressionContainers)
		{
			foreach (IGrouping<string, ExpressionContainer> expressions in expressionContainers.GroupBy(x => x.TargetObject))
			{
				const string attributeName = "CommandParameter";
				Regex commandParameterEventRegex = new Regex("^(?<eventName>[a-zA-Z0-9_]+)\\." + attributeName, RegexOptions.Compiled | RegexOptions.IgnoreCase);
				List<ExpressionContainer> commandParameterEventExpressions = expressions.Where(x => commandParameterEventRegex.IsMatch(x.TargetField)).ToList();
				ExpressionContainer commandParameterExpression = expressions.SingleOrDefault(x => attributeName.Equals(x.TargetField, StringComparison.InvariantCultureIgnoreCase));

				commandParameterEventExpressions.ForEach(x => x.IsCommandParameterExpression = true);
				if (commandParameterExpression != null)
				{
					commandParameterExpression.IsCommandParameterExpression = true;
				}

				foreach (ExpressionContainer expression in commandParameterEventExpressions)
				{
					//find associated event (if exists)
					string eventName = commandParameterEventRegex.Match(expression.TargetField).Groups["eventName"].Value;
					ExpressionContainer associatedExpression = expressions.FirstOrDefault(x => eventName.Equals(x.TargetField, StringComparison.InvariantCultureIgnoreCase) && !x.IsCommandParameterExpression);
					if (associatedExpression != null)
					{
						// create proxy property CommandParameterProxy to handle this
						var result = CodeGeneratorHelper.GenerateProxyProperty(NameGeneratorHelper.GetCommandParameterName(), "CommandParameterProxy");

						Properties.Add(result.Item2);
						Fields.Add(result.Item1);

						string propertyName = CodeGeneratorHelper.GetPropertyReference(result.Item2).PropertyName;

						// retarget the binding expression to this new property and to the Value field
						expression.TargetObject = propertyName;
						expression.TargetField = "Value";

						associatedExpression.CommandParameterTarget = propertyName;
					}
				}

				if (commandParameterExpression != null)
				{
					// create proxy property CommandParameterProxy to handle this
					var result = CodeGeneratorHelper.GenerateProxyProperty(NameGeneratorHelper.GetCommandParameterName(), "CommandParameterProxy");

					Properties.Add(result.Item2);
					Fields.Add(result.Item1);

					string propertyName = CodeGeneratorHelper.GetPropertyReference(result.Item2).PropertyName;

					// retarget the binding expression to this new property and to the Value field
					commandParameterExpression.TargetObject = propertyName;
					commandParameterExpression.TargetField = "Value";

					foreach(ExpressionContainer associatedExpression in expressions.Where(x => string.IsNullOrEmpty(x.CommandParameterTarget) && !x.IsCommandParameterExpression))
					{
						associatedExpression.CommandParameterTarget = propertyName;
					}
				}
			}
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
				DataTemplateResource dataTemplateResource = resource as DataTemplateResource;
				Tuple<CodeMemberField, CodeMemberProperty> result;
				if (dataTemplateResource != null) // in case of data templates
				{
					const string type = "Storm.Mvvm.DataTemplate";
					result = CodeGeneratorHelper.GenerateProxyProperty(resource.PropertyName, type, fieldReference => new List<CodeStatement>
					{
						// _field = new DataTemplate();
						new CodeAssignStatement(fieldReference, new CodeObjectCreateExpression(CodeGeneratorHelper.GetTypeReferenceFromName(type))), 
						// _field.ViewId = Resource.Id.***
						new CodeAssignStatement(new CodePropertyReferenceExpression(fieldReference, "ViewId"), CodeGeneratorHelper.GetAndroidResourceReference(ResourcePart.Layout, dataTemplateResource.ViewId)), 
						// _field.LayoutInflater = LayoutInflater;
						new CodeAssignStatement(new CodePropertyReferenceExpression(fieldReference, "LayoutInflater"), GetLayoutInflaterReference()), 
						// _field.ViewHolderType = typeof(viewholder class)
						new CodeAssignStatement(new CodePropertyReferenceExpression(fieldReference, "ViewHolderType"), new CodeTypeOfExpression(string.Format("{0}.{1}", Configuration.GeneratedNamespace, dataTemplateResource.ViewHolderClassName))),
					});
				}
				else
				{
					// create a proxy property to handle the resource
					string type = resource.Type;
					Dictionary<string, string> assignments = resource.Properties;
					result = CodeGeneratorHelper.GenerateProxyProperty(resource.PropertyName, type, fieldReference => CodeGeneratorHelper.GenerateStatementsCreateAndAssign(fieldReference, type, assignments));
				}
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
