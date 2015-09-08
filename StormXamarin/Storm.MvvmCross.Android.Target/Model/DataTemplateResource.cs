using System.Collections.Generic;
using System.Linq;

namespace Storm.MvvmCross.Android.Target.Model
{
	public class DataTemplateResource : Resource
	{
		public string ViewId { get; set; }

		public string ViewHolderClassName { get; set; }

		public DataTemplateResource()
		{
			
		}

		public DataTemplateResource(Resource resource) : base(resource.Key)
		{
			ResourceElement = resource.ResourceElement.Children.SingleOrDefault();
			foreach (KeyValuePair<string, string> property in resource.Properties)
			{
				Properties.Add(property.Key, property.Value);
			}
		}
	}
}
