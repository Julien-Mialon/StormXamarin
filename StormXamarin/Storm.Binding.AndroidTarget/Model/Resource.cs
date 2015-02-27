using System.Collections.Generic;

namespace Storm.Binding.AndroidTarget.Model
{
	public class Resource
	{
		public string Key { get; set; }

		public Dictionary<string, string> Properties { get; private set; }

		public XmlElement ResourceElement { get; set; }

		public Resource()
		{
			Properties = new Dictionary<string, string>();
		}

		public Resource(string key) : this()
		{
			Key = key;
		}
	}
}
