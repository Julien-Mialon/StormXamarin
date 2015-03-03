namespace Storm.Binding.AndroidTarget.Model
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
