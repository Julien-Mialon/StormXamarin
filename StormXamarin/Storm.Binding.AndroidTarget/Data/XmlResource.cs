using System.Collections.Generic;

namespace Storm.Binding.AndroidTarget.Data
{
	public abstract class XmlResource
	{
		public enum Type
		{
			Converter,
			DataTemplate,
			ViewSelector,
		}

		public Type ResourceType { get; set; }

		public string Key { get; set; }

		protected XmlResource(Type type)
		{
			ResourceType = type;
		}

		protected XmlResource(Type type, string key) : this(type)
		{
			Key = key;
		}
	}

	public class ResourceConverter : XmlResource
	{
		public string ClassName { get; set; }

		public ResourceConverter(string key, string className) : base(Type.Converter, key)
		{
			ClassName = className;
		}
	}

	public class ResourceDataTemplate : XmlResource
	{
		public ViewInfo ViewInformation { get; set; }
		public XmlElement RootElement { get; set; }

		public string ResourceId { get; set; }

		public ResourceDataTemplate(string key, XmlElement rootElement, ViewInfo viewInformation) : base(Type.DataTemplate, key)
		{
			RootElement = rootElement;
			ViewInformation = viewInformation;
		}
	}

	public class ResourceViewSelector : XmlResource
	{
		private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();
		
		public string ClassName { get; set; }

		public Dictionary<string, string> Properties { get { return _properties; } } 

		public ResourceViewSelector(string key, string className) : base(Type.ViewSelector, key)
		{
			ClassName = className;
		}
	}
}
