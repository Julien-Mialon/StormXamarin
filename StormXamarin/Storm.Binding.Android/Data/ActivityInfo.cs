using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Binding.Android.Data
{
	public class ActivityInfo
	{
		public string inputFile { get; set; }
		public string outputFile { get; set; }
		public string className { get; set; }
		public string namespaceName { get; set; }
	}

	public class ViewInfo
	{
		public string inputFile { get; set; }
		public string outputFile { get; set; }
	}

	public class BindingInfo
	{
		public ActivityInfo activity { get; set; }
		public ViewInfo view { get; set; }
	}

	public class BindingInfoCollection
	{
		public List<BindingInfo> list { get; set; }
	}
}
