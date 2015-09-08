using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Storm.MvvmCross.Android.Target.Compiler;
using Storm.MvvmCross.Android.Target.Helper;
using Storm.MvvmCross.Android.Target.Model;
using XmlAttribute = Storm.MvvmCross.Android.Target.Model.XmlAttribute;
using XmlElement = Storm.MvvmCross.Android.Target.Model.XmlElement;

namespace Storm.MvvmCross.Android.Target.Preprocessor
{
	public class ViewFileWriter
	{
		private readonly Dictionary<string, string> _nsDictionary = new Dictionary<string, string>();

		public void Write(XmlElement root, string outputFile, List<StyleResource> styleResources)
		{
			XmlWriterSettings settings = new XmlWriterSettings
			{
				Encoding = Encoding.UTF8,
				Indent = true,
				IndentChars = "\t",
				NewLineChars = "\n",
				NewLineHandling = NewLineHandling.Replace
			};
			_nsDictionary.Clear();
			using (XmlWriter writer = XmlWriter.Create(outputFile, settings))
			{
				writer.WriteStartDocument();

				WriteElement(writer, root, styleResources);

				writer.WriteEndDocument();
			}
		}

		private void WriteElement(XmlWriter writer, XmlElement element, List<StyleResource> styleResources)
		{
			writer.WriteStartElement(ToLowerNamespace(element.LocalName));
			foreach (XmlAttribute attr in element.Attributes)
			{
				WriteAttribute(writer, attr, styleResources);
			}

			foreach (XmlElement child in element.Children)
			{
				WriteElement(writer, child, styleResources);
			}
			writer.WriteEndElement();
		}

		private void WriteAttribute(XmlWriter writer, XmlAttribute attribute, List<StyleResource> styleResources)
		{
			if (ParsingHelper.IsXmlOnlyAttribute(attribute))
			{
				WriteSpecificAttribute(writer, attribute, styleResources);
			}
			else
			{
				if (!string.IsNullOrWhiteSpace(attribute.NamespaceUri))
				{
					string namespaceUri = attribute.NamespaceUri;
					string localName = attribute.LocalName;
					string namespaceName = attribute.FullName.Split(':')[0];

					if (_nsDictionary.ContainsKey(namespaceUri))
					{
						namespaceName = _nsDictionary[namespaceUri];
					}
					else
					{
						_nsDictionary.Add(namespaceUri, namespaceName);
					}

					writer.WriteAttributeString(namespaceName, localName, namespaceUri, attribute.Value);
				}
				else
				{
					writer.WriteAttributeString(attribute.FullName, attribute.Value);
				}
			}
		}

		private void WriteSpecificAttribute(XmlWriter writer, XmlAttribute attribute, List<StyleResource> styleResources)
		{
			if (ParsingHelper.IsStyleAttribute(attribute))
			{
				BindingLanguageParser compiler = new BindingLanguageParser();
				bool result;
				Expression resultExpression = compiler.Parse(attribute.Value, out result);

				if (!result)
				{
					throw new CompileException(string.Format("Can not compile expression {0}", attribute.Value));
				}

				if (!resultExpression.IsOfType(ExpressionType.Resource))
				{
					throw new CompileException(string.Format("Expecting resource expression for style, got {0}", attribute.Value));
				}

				string resourceKey = resultExpression.GetValue(ResourceExpression.KEY);
				//find correct resource
				StyleResource styleResource = styleResources.FirstOrDefault(x => resourceKey.Equals(x.Key, StringComparison.InvariantCultureIgnoreCase));

				if (styleResource == null)
				{
					throw new IndexOutOfRangeException(string.Format("Resource with key {0} does not exists", resourceKey));
				}

				foreach (XmlAttribute attr in styleResource.ResourceElement.Attributes.Where(x => !ParsingHelper.IsResourceKeyAttribute(x)))
				{
					// write all attributes embedded in the style
					WriteAttribute(writer, attr, styleResources);
				}
			}
		}

		private string ToLowerNamespace(string input)
		{
			if (input.Contains("."))
			{
				int lastPosition = input.LastIndexOf('.');
				string namespaceName = input.Substring(0, lastPosition);
				string className = input.Substring(lastPosition);

				input = namespaceName.ToLowerInvariant() + className;
			}
			return input;
		}
	}
}
