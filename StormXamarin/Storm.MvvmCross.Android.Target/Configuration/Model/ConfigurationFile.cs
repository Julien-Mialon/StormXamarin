using System.Collections.Generic;

namespace Storm.MvvmCross.Android.Target.Configuration.Model
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

		public List<string> GlobalResourceFiles { get; private set; } 

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

		/// <summary>
		/// Name of the class to use as default TemplateSelector if none is provided in Binding expression.
		/// This class has to implement ITemplateSelector.
		/// </summary>
		public string DefaultTemplateSelector { get; set; }

		/// <summary>
		/// Name of the field to assign the Template to the DefaultTemplateSelector class.
		/// This field has to be of type int.
		/// </summary>
		public string DefaultTemplateSelectorField { get; set; }

		/// <summary>
		/// Name of the class to use as default Adapter if none is provided in Binding expression.
		/// This class has to implement IMvvmAdapter.
		/// </summary>
		public string DefaultAdapter { get; set; }

		/// <summary>
		/// Name of the field to assign the TemplateSelector to the DefaultAdapter class.
		/// This field has to be of type ITemplateSelector.
		/// </summary>
		public string DefaultAdapterField { get; set; }

		/// <summary>
		/// Boolean to enable or disable the case sensitivity in property name in xml files for binding properties.
		/// Enabling this will have better performance but you can not use "as is" old android xml view file since property now have an upper case first letter.
		/// </summary>
		public bool? CaseSensitivity { get; set; }

		public ConfigurationFile()
		{
			Namespaces = new List<string>();
			Aliases = new List<AliasDescription>();
			FileDescriptions = new List<FileBindingDescription>();
			GlobalResourceFiles = new List<string>();
		}
	}
}