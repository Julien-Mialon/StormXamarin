using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Binding.Android.Data
{
	public class XmlAttribute
	{
		public string Name { get; set; }

		public string Value { get; set; }

		public string LocalName
		{
			get
			{
				if (Name.Contains(':'))
				{
					string[] splitted = Name.Split(':');
					return splitted[1];
				}
				else
				{
					return Name;
				}
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
