using System.Collections.Generic;

namespace Storm.Binding.Android.Data
{
	public class ActivityInfo
	{
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public string ClassName { get; set; }
		public string NamespaceName { get; set; }
	}

	public class ViewInfo
	{
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
	}

	public class ActivityViewInfo
	{
		public ActivityInfo Activity { get; set; }
		public ViewInfo View { get; set; }
	}

	public class ActivityViewInfoCollection
	{
		public List<ActivityViewInfo> List { get; set; }
	}
}
