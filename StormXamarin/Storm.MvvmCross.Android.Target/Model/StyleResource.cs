using System.Collections.Generic;

namespace Storm.MvvmCross.Android.Target.Model
{
	public class StyleResource : Resource
	{
		public StyleResource()
		{
			
		}

		public StyleResource(Resource resource) : base(resource.Key)
		{
			ResourceElement = resource.ResourceElement;
			foreach (KeyValuePair<string, string> property in resource.Properties)
			{
				Properties.Add(property.Key, property.Value);
			}
		}
	}
}
