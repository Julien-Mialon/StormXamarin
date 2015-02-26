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
}
