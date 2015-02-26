using Newtonsoft.Json;

namespace Storm.Binding.AndroidTarget.Configuration.Model
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


		// Fields below are for processing and should not be used in json file !

		[JsonIgnore]
		public string OutputFile { get; set; }

		/// <summary>
		/// Useful for processing and to know if the current instance is a fragment or an Activity
		/// </summary>
		[JsonIgnore]
		public virtual bool IsFragment { get { return false; } }
	}
}
