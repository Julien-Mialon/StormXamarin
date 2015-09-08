using Newtonsoft.Json;

namespace Storm.MvvmCross.Android.Target.Configuration.Model
{
	//Store information about Activity file
	public class ActivityDescription
	{
		/// <summary>
		/// Name of the class
		/// </summary>
		public string ClassName { get; set; }
		/// <summary>
		/// Namespace where the class is
		/// </summary>
		public string NamespaceName { get; set; }

		/// <summary>
		/// Name of the outputFile, could be null, a name will be auto generated
		/// </summary>
		public string OutputFile { get; set; }

		// Fields below are for processing and should not be used in json file !

		/// <summary>
		/// Useful for processing and to know if the current instance is a fragment or an Activity
		/// </summary>
		[JsonIgnore]
		public virtual bool IsFragment { get { return false; } }
	}
}
