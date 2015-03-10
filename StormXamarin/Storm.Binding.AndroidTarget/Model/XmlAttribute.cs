using System.Linq;

namespace Storm.Binding.AndroidTarget.Model
{
	public class XmlAttribute
	{
		public string FullName { get; set; }

		public string Value { get; set; }

		public string NamespaceUri { get; set; }

		public string LocalName
		{
			get
			{
				if (!FullName.Contains(':'))
				{
					return FullName;
				}
				string[] splitted = FullName.Split(':');
				return splitted[1];
			}
		}

		public string AttachedId { get; set; }
	}
}