using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Storm.Binding.AndroidTarget.Compiler;
using Storm.Binding.AndroidTarget.Model;

namespace Storm.Binding.AndroidTarget.Process
{
	public static class ClassGeneratorHelper
	{
		const string RESOURCE_ACCESS_START = "{Resource ";
		const string RESOURCE_ACCESS_END = "}";

		const string BINDING_EXPRESSION_START = "{Binding ";
		const string BINDING_EXPRESSION_END = "}";

		const string BINDING_EXPRESSION_EMPTY = "{Binding}";

		public static readonly List<string> DefaultNamespaces = new List<string>
		{
			"System",
			"System.Collections.Generic",
			"System.Reflection",
			"Android.App",
			"Android.Content",
			"Android.Runtime",
			"Android.Views",
			"Android.Widget",
			"Android.OS",
			"Storm.Mvvm",
			"Storm.Mvvm.Bindings",
			"Storm.Mvvm.Components",
			"Storm.Mvvm.ViewSelectors",
		};

		public static bool IsResourceReferenceExpression(string input)
		{
			input = input.Trim();
			return (input.StartsWith(RESOURCE_ACCESS_START) && input.EndsWith(RESOURCE_ACCESS_END));
		}

		public static string ExtractResourceKey(string input)
		{
			input = input.Trim();
			if (input.StartsWith(RESOURCE_ACCESS_START) && input.EndsWith(RESOURCE_ACCESS_END))
			{
				input = input.Substring(RESOURCE_ACCESS_START.Length);
				input = input.Substring(0, input.Length - RESOURCE_ACCESS_END.Length).Trim();

				return input;
			}
			return null;
		}

		public static BindingExpression EvaluateBindingExpression(XmlAttribute attribute, Dictionary<string, CodePropertyReferenceExpression> resources)
		{
			BindingLanguageParser parser = new BindingLanguageParser();
			bool parsingResult;
			Expression result = parser.Parse(attribute.Value, out parsingResult);
			BindingPreprocess.LexLog("");

			string bindingValue = attribute.Value;
			if (bindingValue.StartsWith(BINDING_EXPRESSION_START) && bindingValue.EndsWith(BINDING_EXPRESSION_END))
			{
				bindingValue = bindingValue.Substring(BINDING_EXPRESSION_START.Length);
				bindingValue = bindingValue.Substring(0, bindingValue.Length - BINDING_EXPRESSION_END.Length).Trim();

				//Format des bindings expression
				/*{Binding [Path=]Path[, Converter={Resource ConverterKey}[, ConverterParameter=(VALUE|'SPACED VALUE')]][, Mode=(OneTime|OneWay|TwoWay)[, UpdateEvent=EventName][, Adapter={Resource ViewSelectorKey}]
				 * Le nom de l'attribut Path est optionel
				 * ConverterParameter n'est autorisé que si il y a un converter
				 * UpdateEvent n'est autorisé qu'avec un mode "TwoWay"
				 */
				BindingExpression expression = new BindingExpression
				{
					TargetFieldId = attribute.LocalName,
					TargetObjectId = attribute.AttachedId
				};
				bool pathFound = false;
				Regex pattern = new Regex("^([a-zA-Z0-9]+) ?= ?(.+)$");
				foreach (string attr in bindingValue.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()))
				{
					string attributeName;
					string attributeValue;

					if (pattern.IsMatch(attr))
					{
						Match matchResult = pattern.Match(attr);
						attributeName = matchResult.Groups[1].Value;
						attributeValue = matchResult.Groups[2].Value;
					}
					else //on suppose que c'est le path
					{
						attributeName = "Path";
						attributeValue = attr;
					}

					if (attributeName == "Path")
					{
						if (pathFound)
						{
							BindingPreprocess.Logger.LogError("Binding error : missing attribute name in " + bindingValue);
							return null;
						}
						pathFound = true;

						expression.SourcePath = attributeValue;
					}
					else if (attributeName == "Converter")
					{
						// need to parse {Resource ... }
						if (IsResourceReferenceExpression(attributeValue))
						{
							string resourceKey = ExtractResourceKey(attributeValue);

							if (resources.ContainsKey(resourceKey))
							{
								expression.ConverterReference = resources[resourceKey];
							}
							else
							{
								BindingPreprocess.Logger.LogError("Binding error : no converter in ressource for " + bindingValue);
							}
						}
						else
						{
							BindingPreprocess.Logger.LogError("Binding Error : invalid converter " + bindingValue);
							return null;
						}
					}
					else if (attributeName == "ConverterParameter")
					{
						if (attributeValue.StartsWith("'") && attributeValue.EndsWith("'"))
						{
							attributeValue = attributeValue.Substring(1, attributeValue.Length - 2).Replace("\\\\", "\\").Replace("\\'", "'");
						}
						expression.ConverterParameter = attributeValue;
					}
					else if (attributeName == "Mode")
					{
						BindingExpression.BindingModes enumResult;
						if (Enum.TryParse(attributeValue, false, out enumResult))
						{
							expression.Mode = enumResult;
						}
						else
						{
							BindingPreprocess.Logger.LogError("Binding error : unrecognized Binding Mode " + attributeValue);
							return null;
						}
					}
					else if (attributeName == "UpdateEvent")
					{
						expression.UpdateEvent = attributeValue;
					}
					else if (attributeName == "ViewSelector")
					{
						// need to parse {Resource ... }
						if (IsResourceReferenceExpression(attributeValue))
						{
							string resourceKey = ExtractResourceKey(attributeValue);

							if (resources.ContainsKey(resourceKey))
							{
								expression.ViewSelectorReference = resources[resourceKey];
							}
							else
							{
								BindingPreprocess.Logger.LogError("Binding error : no viewselector in ressource for " + bindingValue);
							}
						}
						else
						{
							BindingPreprocess.Logger.LogError("Binding Error : invalid viewselector " + bindingValue);
							return null;
						}
					}
					else
					{
						BindingPreprocess.Logger.LogError("Unrecognized attribute {0} in binding expression {1}", attributeName, bindingValue);
					}
				}

				if (!pathFound)
				{
					expression.SourcePath = "";
				}

				if (expression.ConverterReference == null && !string.IsNullOrWhiteSpace(expression.ConverterParameter))
				{
					BindingPreprocess.Logger.LogError("Binding Error : ConverterParameter is not authorized if no converter is specified " + bindingValue);
					//Console.WriteLine("Binding Error : ConverterParameter is not authorized if no converter is specified " + bindingValue);
					return null;
				}

				if (expression.Mode != BindingExpression.BindingModes.TwoWay && !string.IsNullOrWhiteSpace(expression.UpdateEvent))
				{
					BindingPreprocess.Logger.LogError("Binding Error : UpdateEvent is not authorized if Mode is not TwoWay " + bindingValue);
					//Console.WriteLine("Binding Error : UpdateEvent is not authorized if Mode is not TwoWay " + bindingValue);
					return null;
				}

				if (expression.Mode == BindingExpression.BindingModes.TwoWay && string.IsNullOrWhiteSpace(expression.UpdateEvent))
				{
					BindingPreprocess.Logger.LogError("Binding error : missing update event for two way binding : " + bindingValue);
					//Console.WriteLine("Binding error : missing update event for two way binding : " + bindingValue);
					return null;
				}

				return expression;
			}

			if (bindingValue == BINDING_EXPRESSION_EMPTY)
			{
				return new BindingExpression
				{
					TargetFieldId = attribute.LocalName,
					TargetObjectId = attribute.AttachedId, 
					SourcePath = ""
				};
			}

			BindingPreprocess.Logger.LogError("Error in binding expression : " + attribute.Value);
			return null;
		}
	}
}
