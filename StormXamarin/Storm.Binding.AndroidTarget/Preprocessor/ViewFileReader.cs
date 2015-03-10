using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Storm.Binding.AndroidTarget.Helper;
using XmlAttribute = Storm.Binding.AndroidTarget.Model.XmlAttribute;
using XmlElement = Storm.Binding.AndroidTarget.Model.XmlElement;

namespace Storm.Binding.AndroidTarget.Preprocessor
{
	/// <summary>
	/// Class to handle reading of xml view files
	/// </summary>
	public class ViewFileReader
	{
		private readonly Dictionary<string, string> _aliases;

		/// <summary>
		/// Create a ViewFileReader with the list of defined aliases.
		/// </summary>
		/// <param name="aliases">list of aliases</param>
		public ViewFileReader(Dictionary<string, string> aliases)
		{
			_aliases = aliases;
		}

		/// <summary>
		/// Function to read a view file and handle aliases, using: namespace on the fly replacement
		/// </summary>
		/// <param name="filePath">the path of the file to read</param>
		/// <returns>root xml element</returns>
		public XmlElement Read(string filePath)
		{
			Stack<XmlElement> elements = new Stack<XmlElement>();
			XmlElement current = null;

			using (XmlReader reader = XmlReader.Create(filePath))
			{
				bool continueWithoutRead = false;
				while (continueWithoutRead || reader.Read())
				{
					continueWithoutRead = false;
					if (reader.NodeType == XmlNodeType.Element)
					{
						bool isAutoClose = reader.IsEmptyElement;

						string elementName;
						string namespacePrefix = "";
						//no xml namespace
						if (string.IsNullOrWhiteSpace(reader.NamespaceURI))
						{
							elementName = reader.Name;
							if (_aliases.ContainsKey(elementName))
							{
								elementName = _aliases[elementName];
							}
						}
						else if (ParsingHelper.IsUsingUri(reader.NamespaceURI))
						{
							elementName = string.Format("{0}.{1}", ParsingHelper.GetUsingNamespace(reader.NamespaceURI), reader.LocalName);
						}
						else
						{
							namespacePrefix = reader.Prefix;
							elementName = reader.LocalName;
						}

						XmlElement childElement = new XmlElement
						{
							NamespaceName = namespacePrefix,
							LocalName = elementName,
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
								if (reader.Prefix != "xmlns" || !ParsingHelper.IsUsingUri(reader.Value))
								{
									childElement.Attributes.Add(new XmlAttribute
									{
										NamespaceUri = reader.NamespaceURI,
										FullName = reader.Name, 
										Value = reader.Value
									});
								}
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
	}
}
