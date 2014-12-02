using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using Storm.Binding.Android.Data;
using XmlAttribute = Storm.Binding.Android.Data.XmlAttribute;
using XmlElement = Storm.Binding.Android.Data.XmlElement;

namespace Storm.Binding.Android.Process
{
	class ViewFileProcessor
	{
		private readonly Dictionary<string, string> _nsDictionary = new Dictionary<string, string>(); 

		public List<IdViewObject> Views = new List<IdViewObject>();

		private int _processedItems = 1;

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
						bool isAutoClose = reader.IsEmptyElement;

						XmlElement childElement = new XmlElement
						{
							Name = reader.Name,
						};

						if (current != null)
						{
							current.Children.Add(childElement);
							if (!isAutoClose)
							{
								elements.Push(current);
							}
						}

						if (reader.HasAttributes)
						{
							reader.MoveToFirstAttribute();
							do
							{
								childElement.Attributes.Add(new XmlAttribute {Name = reader.Name, Value = reader.Value});
							} while (reader.MoveToNextAttribute());
						}

						reader.Read();
						continueWithoutRead = !reader.HasValue;

						if (!isAutoClose)
						{
							current = childElement;
						}
					}
					else if (reader.NodeType == XmlNodeType.EndElement)
					{
						if (elements.Any())
						{
							current = elements.Pop();
						}
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
			if (element.Name != "Resources")
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
		}
		
		private void WriteAttribute(XmlWriter writer, XmlAttribute attribute)
		{
			if (attribute.Name.Contains(":"))
			{
				string[] splitted = attribute.Name.Split(':');
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

		public List<XmlResource> ExtractResources(XmlElement element)
		{
			if (element.Name == "Resources")
			{
				//Process each child to extract resources
				List<XmlResource> resources = new List<XmlResource>();
				foreach (XmlElement child in element.Children)
				{
					if (child.Name == "Converter")
					{
						XmlAttribute keyAttribute = child.Attributes.SingleOrDefault(x => x.Name == "Key");
						XmlAttribute classAttribute = child.Attributes.SingleOrDefault(x => x.Name == "Class");

						if (keyAttribute == null || classAttribute == null)
						{
							throw new Exception("Missing attribute for converter : key = " + ((keyAttribute == null) ? "null" : keyAttribute.Value) + " class = " + ((classAttribute == null) ? "null" : classAttribute.Value));
						}

						resources.Add(new ResourceConverter(keyAttribute.Value, classAttribute.Value));

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
				return element.Children.Select(ExtractResources).FirstOrDefault(res => res != null);
			}

			return new List<XmlResource>();
		}

		public Tuple<List<XmlAttribute>, List<IdViewObject>> ExtractBindingInformations(XmlElement element)
		{
			Views.Clear();

			List<XmlAttribute> attributes = _ExtractBindingInformations(element);

			return new Tuple<List<XmlAttribute>, List<IdViewObject>>(attributes, Views.ToList());
		}

		private List<XmlAttribute> _ExtractBindingInformations(XmlElement element)
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

					Views.Add(new IdViewObject(element.Name, id));
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
				XmlAttribute attribute = new XmlAttribute();
				id = "ZAutoGenerated_" + GetSha256Hash(_processedItems + element.Name + string.Join("#",element.Attributes.Select(x => x.Name + x.Value)));
				_processedItems++;
				attribute.Value = "@+id/" + id;
				attribute.Name = "android:id";

				element.Attributes.Add(attribute);
				Views.Add(new IdViewObject(element.Name, id));
			}

			foreach (XmlAttribute attribute in bindings)
			{
				attribute.AttachedId = id;
				element.Attributes.Remove(attribute);
			}

			foreach (XmlElement child in element.Children)
			{
				bindings.AddRange(_ExtractBindingInformations(child));
			}

			return bindings;
		}

		private static string GetSha256Hash(string input, int length = 64)
		{
			byte[] bytes = Encoding.Unicode.GetBytes(input);
			SHA256Managed hashstring = new SHA256Managed();
			byte[] hash = hashstring.ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte x in hash)
			{
				stringBuilder.AppendFormat("{0:x2}", x);
			}
			return stringBuilder.ToString().Substring(0, length);
		}
	}

	public class IdViewObject
	{
		public string TypeName { get; set; }

		public string Id { get; set; }

		public IdViewObject()
		{
			
		}

		public IdViewObject(string typeName, string id)
		{
			TypeName = typeName;
			Id = id;
		}
	}
}
