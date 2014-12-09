using System.Collections.Generic;

namespace Storm.Binding.Android.Data
{
	public class ActivityInfo
	{
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
		public string ClassName { get; set; }
		public string NamespaceName { get; set; }

		public virtual bool IsFragment { get { return false; } }
	}

	public class FragmentInfo : ActivityInfo
	{
		public override bool IsFragment { get { return true; } }
	}

	public class ViewInfo
	{
		public string InputFile { get; set; }
		public string OutputFile { get; set; }
	}

	public class ActivityViewInfo
	{
		public FragmentInfo Fragment { set { Activity = value; } }
		public ActivityInfo Activity { get; set; }
		public ViewInfo View { get; set; }
		public List<ViewInfo> Adapters { get; private set; }

		public ActivityViewInfo()
		{
			Adapters = new List<ViewInfo>();
		}
	}

	public class ActivityViewInfoCollection
	{
		public List<ActivityViewInfo> List { get; set; }
	}
}
