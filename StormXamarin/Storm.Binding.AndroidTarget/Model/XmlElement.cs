using System.Collections.Generic;

namespace Storm.Binding.AndroidTarget.Model
{
	public class XmlElement
	{
		public string NamespaceName { get; set; }

		public string LocalName { get; set; }

		public Dictionary<string, string> Namespaces { get; private set; } 

		public List<XmlAttribute> Attributes { get; private set; }

		public List<XmlElement> Children { get; private set; }

		public XmlElement()
		{
			Namespaces = new Dictionary<string, string>();
			Attributes = new List<XmlAttribute>();
			Children = new List<XmlElement>();
		}
	}
}
