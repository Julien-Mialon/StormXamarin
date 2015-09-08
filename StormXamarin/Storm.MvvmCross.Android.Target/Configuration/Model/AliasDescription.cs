namespace Storm.MvvmCross.Android.Target.Configuration.Model
{
	/// <summary>
	/// This class define alias between class name and simple name to be used in xml files.
	/// </summary>
	public class AliasDescription
	{
		/// <summary>
		/// The simplified name
		/// </summary>
		public string Alias { get; set; }

		/// <summary>
		/// The complete name
		/// </summary>
		public string FullClassName { get; set; }

		public AliasDescription()
		{
			
		}

		public AliasDescription(string @alias, string fullClassName)
		{
			Alias = alias;
			FullClassName = fullClassName;
		}
	}
}