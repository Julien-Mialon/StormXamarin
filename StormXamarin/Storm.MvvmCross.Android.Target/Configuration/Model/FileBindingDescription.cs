namespace Storm.MvvmCross.Android.Target.Configuration.Model
{
	/// <summary>
	/// File to store reference between View and Activity (or Fragment) files
	/// </summary>
	public class FileBindingDescription
	{
		/// <summary>
		/// Store information about view file
		/// </summary>
		public ViewDescription View { get; set; }
		
		/// <summary>
		/// Store information about activity file
		/// </summary>
		public ActivityDescription Activity { get; set; }

		/// <summary>
		/// Only setter to assign Activity with correct object when reading json file
		/// </summary>
		public FragmentDescription Fragment { set { Activity = value; } }
	}
}