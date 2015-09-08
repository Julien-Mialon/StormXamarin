namespace Storm.MvvmCross.Android.Target.Model
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
