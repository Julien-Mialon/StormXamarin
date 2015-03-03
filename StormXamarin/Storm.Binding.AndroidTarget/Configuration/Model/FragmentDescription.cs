using Newtonsoft.Json;

namespace Storm.Binding.AndroidTarget.Configuration.Model
{
	/// <summary>
	/// Fragment work exactly as Activity
	/// </summary>
	public class FragmentDescription : ActivityDescription
	{
		/// <summary>
		/// Override to specify it's a fragment and not a regular activity
		/// </summary>
		[JsonIgnore]
		public override bool IsFragment { get { return true; } }
	}
}