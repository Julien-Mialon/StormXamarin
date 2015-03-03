using System.Collections.Generic;
using System.Text;
using System.Xml;
using XmlAttribute = Storm.Binding.AndroidTarget.Model.XmlAttribute;
using XmlElement = Storm.Binding.AndroidTarget.Model.XmlElement;

namespace Storm.Binding.AndroidTarget.Preprocessor
{
	public class ViewFileWriter
	{
		private readonly Dictionary<string, string> _nsDictionary = new Dictionary<string, string>();

		public void Write(XmlElement root, string outputFile)
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

				WriteElement(writer, root);

				writer.WriteEndDocument();
			}
		}

		private void WriteElement(XmlWriter writer, XmlElement element)
		{
			writer.WriteStartElement(ToLowerNamespace(element.LocalName));
			foreach (XmlAttribute attr in element.Attributes)
			{
				WriteAttribute(writer, attr);
			}

			foreach (XmlElement child in element.Children)
			{
				WriteElement(writer, child);
			}
			writer.WriteEndElement();
		}

		private void WriteAttribute(XmlWriter writer, XmlAttribute attribute)
		{
			if (attribute.FullName.Contains(":"))
			{
				string[] splitted = attribute.FullName.Split(':');
				string ns = splitted[0];
				string name = splitted[1];

				if (ns == "xmlns")
				{
					_nsDictionary.Add(name, attribute.Value);
				}
				else
				{
					writer.WriteAttributeString(ns, name, _nsDictionary[ns], attribute.Value);
				}
			}
			else
			{
				writer.WriteAttributeString(attribute.FullName, attribute.Value);
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
