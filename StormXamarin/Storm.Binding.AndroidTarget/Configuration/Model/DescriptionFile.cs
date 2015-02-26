using System.Collections.Generic;

namespace Storm.Binding.AndroidTarget.Configuration.Model
{
	/// <summary>
	/// Root object to store data from configuration file
	/// </summary>
	public class DescriptionFile
	{
		/// <summary>
		/// List of additional namespaces to include into generated files
		/// </summary>
		public List<string> Namespaces { get; private set; }

		/// <summary>
		/// List of defined aliases to simplify xml view writing
		/// </summary>
		public List<AliasDescription> Aliases { get; private set; } 

		/// <summary>
		/// List of binding between activities and views
		/// </summary>
		public List<FileBindingDescription> FileDescriptions { get; private set; }

		public DescriptionFile()
		{
			Namespaces = new List<string>();
			Aliases = new List<AliasDescription>();
			FileDescriptions = new List<FileBindingDescription>();
		}
	}
}