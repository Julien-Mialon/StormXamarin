namespace Storm.Binding.AndroidTarget.Configuration.Model
{
	/// <summary>
	/// This class store information about view files
	/// </summary>
	public class ViewDescription
	{
		/// <summary>
		/// Path to the input xml file
		/// </summary>
		public string InputFile { get; set; }
		/// <summary>
		/// Path to the ouput axml file
		/// </summary>
		public string OutputFile { get; set; }
	}
}