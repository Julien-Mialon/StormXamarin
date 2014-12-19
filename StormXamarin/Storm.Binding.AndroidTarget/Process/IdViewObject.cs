namespace Storm.Binding.AndroidTarget.Process
{
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
