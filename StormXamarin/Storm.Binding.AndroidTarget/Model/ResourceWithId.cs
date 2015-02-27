using System.Collections.Generic;

namespace Storm.Binding.AndroidTarget.Model
{
	public class ResourceWithId : Resource
	{
		public string ResourceId { get; set; }

		public ResourceWithId()
		{
			
		}

		public ResourceWithId(Resource resource) : base(resource.Key)
		{
			ResourceElement = resource.ResourceElement;
			foreach (KeyValuePair<string, string> property in resource.Properties)
			{
				Properties.Add(property.Key, property.Value);
			}
		}
	}
}
