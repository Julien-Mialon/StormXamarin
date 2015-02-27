using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.CodeGenerator.Model;
using Storm.Binding.AndroidTarget.Compiler;
using Storm.Binding.AndroidTarget.Helper;
using Storm.Binding.AndroidTarget.Model;

namespace Storm.Binding.AndroidTarget.CodeGenerator
{
	public class AbstractBindingHandlerClassGenerator : AbstractClassGenerator
	{
		private readonly BindingLanguageParser _expressionParser = new BindingLanguageParser();

		private TaskLoggingHelper Log { get { return BindingPreprocess.Logger; } }

		public void Preprocess(List<XmlAttribute> expressionAttributes, List<Resource> resources)
		{
			// Eval all expressions
			List<ExpressionContainer> expressions = (from attribute in expressionAttributes
													let expressionResult = EvaluateExpression(attribute.Value)
													where expressionResult != null
													 select new ExpressionContainer
													{
														Expression = expressionResult,
														TargetObject = attribute.AttachedId,
														TargetField = attribute.LocalName
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
							Log.LogError("Expression {0} is invalid in a resource context", propertyItem.Value);
						}
						else
						{
							expressions.Add(new ExpressionContainer
							{
								Expression = expr,
								TargetObject = res.PropertyName,
								TargetField = propertyItem.Key,
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
			
			// TODO create property/fields for resources
			
		}

		private List<ExpressionContainer> CreatePropertiesForResources(IEnumerable<Resource> resources)
		{
			List<ExpressionContainer> expressions = new List<ExpressionContainer>();

			foreach (Resource resource in resources)
			{
				if (resource is ResourceWithId)
				{
					string id = ((ResourceWithId) resource).ResourceId;


				}
			}

			return expressions;
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
	}
}
