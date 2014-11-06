using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using XmlAttribute = Storm.Binding.Android.Data.XmlAttribute;
using XmlElement = Storm.Binding.Android.Data.XmlElement;

namespace Storm.Binding.Android.Process
{
	class ViewFileProcessor
	{
		private readonly Dictionary<string, string> _nsDictionary = new Dictionary<string, string>(); 

		public XmlElement Read(string fileName)
		{
			Stack<XmlElement> elements = new Stack<XmlElement>();
			XmlElement current = null;

			using (XmlReader reader = XmlReader.Create(fileName))
			{
				bool continueWithoutRead = false;
				while (continueWithoutRead || reader.Read())
				{
					continueWithoutRead = false;
					if (reader.NodeType == XmlNodeType.Element)
					{
						if (current != null)
						{
							elements.Push(current);
						}
						current = new XmlElement { Name = reader.Name };

						if (elements.Any())
						{
							elements.Peek().Children.Add(current);
						}

						if (reader.HasAttributes)
						{
							reader.MoveToFirstAttribute();
							do
							{
								current.Attributes.Add(new XmlAttribute {Name = reader.Name, Value = reader.Value});
							} while (reader.MoveToNextAttribute());
						}

						reader.Read();
						continueWithoutRead = !reader.HasValue;
					}
					else if (reader.NodeType == XmlNodeType.EndElement)
					{
						current = elements.Pop();
					}
				}
			}

			return current;
		}

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
			writer.WriteStartElement(element.Name);
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
			if (attribute.Name.Contains(":"))
			{
				string ns = "";
				string name = "";

				string[] splitted = attribute.Name.Split(':');
				ns = splitted[0];
				name = splitted[1];

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
				writer.WriteAttributeString(attribute.Name, attribute.Value);	
			}
		}

		public void Display(XmlElement element, int indent = 0)
		{
			string indentString = new string(' ', indent * 2);
			Console.Write("{0}<{1}", indentString, element.Name);
			if (element.Attributes.Any())
			{
				Console.WriteLine();
				string attributeIndent = new string(' ', (indent + 2) * 2);

				foreach (XmlAttribute attr in element.Attributes)
				{
					Console.WriteLine("{0}{1}=\"{2}\"", attributeIndent, attr.Name, attr.Value);
				}

				Console.WriteLine(element.Children.Any() ? "{0}>" : "{0}/>", attributeIndent);
			}
			else
			{
				Console.WriteLine(element.Children.Any() ? ">" : "/>");
			}

			if (element.Children.Any())
			{
				foreach (XmlElement child in element.Children)
				{
					Display(child, indent + 1);
				}
				Console.WriteLine("{0}</{1}>", indentString, element.Name);
			}
		}

		public List<XmlAttribute> ExtractBindingInformations(XmlElement element)
		{
			List<XmlAttribute> bindings = new List<XmlAttribute>();
			string id = null;

			foreach (XmlAttribute attribute in element.Attributes)
			{
				if (attribute.LocalName == "id")
				{
					if (id != null)
					{
						throw new Exception("Multiple id for same element");
					}
					id = attribute.Value;
					attribute.Value = "@+id/" + id;
				}
				else
				{
					if (attribute.Value.Trim().StartsWith("{Binding"))
					{
						bindings.Add(attribute);
					}
				}
			}

			if (id == null && bindings.Any())
			{
				throw new Exception("Missing ID on element with binding");
			}

			foreach (XmlAttribute attribute in bindings)
			{
				attribute.AttachedId = id;
				element.Attributes.Remove(attribute);
			}

			foreach (XmlElement child in element.Children)
			{
				bindings.AddRange(ExtractBindingInformations(child));
			}

			return bindings;
		}
	}
}
