using System.Collections.Generic;
using System.Linq;

namespace Storm.Binding.AndroidTarget.Data
{
	public class XmlAttribute
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public string LocalName
		{
			get
			{
				if (!Name.Contains(':'))
				{
					return Name;
				}
				string[] splitted = Name.Split(':');
				return splitted[1];
			}
		}

		public string AttachedId { get; set; }
	}

	public class XmlElement
	{
		public string Name { get; set; }

		public List<XmlAttribute> Attributes { get; private set; }

		public List<XmlElement> Children { get; private set; }

		public XmlElement()
		{
			Attributes = new List<XmlAttribute>();
			Children = new List<XmlElement>();
		}
	}
}
