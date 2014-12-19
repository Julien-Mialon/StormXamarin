using System;
using System.Collections.Generic;
using System.Linq;
using Storm.Binding.AndroidTarget.Data;

namespace Storm.Binding.AndroidTarget.Process
{
	public static class ResourceParser
	{
		private const string Converter = "Converter";
		private const string ViewSelector = "ViewSelector";
		private const string DataTemplate = "DataTemplate";

		private const string KeyAttribute = "Key";
		private const string ClassAttribute = "Class";

		public static IEnumerable<XmlResource> ParseResources(XmlElement element, ViewInfo viewInformation)
		{
			if (element.Name == "Resources")
			{
				//Process each child to extract resources
				List<XmlResource> resources = new List<XmlResource>();
				foreach (XmlElement child in element.Children)
				{
					if (child.Name == Converter)
					{
						string key = ExtractAttribute(child.Attributes, KeyAttribute);
						string className = ExtractAttribute(child.Attributes, ClassAttribute);

						if (key != null && className != null)
						{
							resources.Add(new ResourceConverter(key, className));
						}
					}
					else if (child.Name == ViewSelector)
					{
						string key = ExtractAttribute(child.Attributes, KeyAttribute);
						string className = ExtractAttribute(child.Attributes, ClassAttribute);

						if (key != null && className != null)
						{
							ResourceViewSelector vs = new ResourceViewSelector(key, className);
							foreach (var attribute in child.Attributes.Where(x => x.Name != KeyAttribute && x.Name != ClassAttribute))
							{
								vs.Properties.Add(attribute.Name, attribute.Value);
							}
							resources.Add(vs);
						}
					}
					else if (child.Name == DataTemplate)
					{
						string key = ExtractAttribute(child.Attributes, KeyAttribute);

						if (key != null)
						{
							if (child.Children.Count == 1)
							{
								ResourceDataTemplate dataTemplate = new ResourceDataTemplate(key, child.Children.First(), viewInformation);
								resources.Add(dataTemplate);
							}
							else
							{
								BindingPreprocess.Logger.LogError("Empty DataTemplate resources");
							}
						}
					}
					else
					{
						throw new Exception("Resource type not supported : " + child.Name);
					}
				}
				return resources;
			}
			if (element.Children.Any())
			{
				return element.Children.SelectMany(x => ParseResources(x, viewInformation));
			}

			return new List<XmlResource>();
		}

		private static string ExtractAttribute(IEnumerable<XmlAttribute> attributes, string attributeName)
		{
			XmlAttribute attribute = attributes.SingleOrDefault(x => x.Name == attributeName);

			if (attribute == null)
			{
				BindingPreprocess.Logger.LogError("Missing attribute {0} in resource", attributeName);
				return null;
			}

			return attribute.Value;
		}
	}
}
