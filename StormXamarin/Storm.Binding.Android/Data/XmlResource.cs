namespace Storm.Binding.Android.Data
{
	public class XmlResource
	{
		public enum Type
		{
			Converter,
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
}
