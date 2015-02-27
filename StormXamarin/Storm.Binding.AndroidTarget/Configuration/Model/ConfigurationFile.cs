using System.Collections.Generic;

namespace Storm.Binding.AndroidTarget.Configuration.Model
{
	/// <summary>
	/// Root object to store data from configuration file
	/// </summary>
	public class ConfigurationFile
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

		/// <summary>
		/// Directory where to store all generated class files
		/// </summary>
		public string ClassLocation { get; set; }

		/// <summary>
		/// Directory where to store all generated resource layouts (usually : $(ProjectDir)/Resources/layout/)
		/// </summary>
		public string ResourceLocation { get; set; }

		/// <summary>
		/// Namespace for all generated classes
		/// </summary>
		public string GeneratedNamespace { get; set; }

		public ConfigurationFile()
		{
			Namespaces = new List<string>();
			Aliases = new List<AliasDescription>();
			FileDescriptions = new List<FileBindingDescription>();
		}
	}
}