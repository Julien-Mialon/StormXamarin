using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Helper;

namespace Storm.Binding.AndroidTarget.Configuration
{
	public class ConfigurationReader
	{
		public string DefaultClassLocation { get; set; }

		public string DefaultResourceLocation { get; set; }

		public ConfigurationFile Read(string inputFile)
		{
			try
			{
				string content = File.ReadAllText(inputFile);

				ConfigurationFile result = JsonConvert.DeserializeObject<ConfigurationFile>(content);

				//Add all default aliases if they have not been overriden
				Dictionary<string, string> aliases = result.Aliases.ToDictionary(x => x.Alias, x => x.FullClassName);
				foreach (AliasDescription alias in DefaultConfiguration.Aliases)
				{
					if (!aliases.ContainsKey(alias.Alias))
					{
						result.Aliases.Add(alias);
					}
				}

				//Add all default namespaces
				foreach (string name in DefaultConfiguration.Namespaces)
				{
					if (!result.Namespaces.Contains(name))
					{
						result.Namespaces.Add(name);
					}
				}
				result.Namespaces.Sort();

				string relativePath = Path.GetDirectoryName(inputFile) ?? "";
				//Process files to complete with full path
				foreach (FileBindingDescription fileBinding in result.FileDescriptions)
				{
					if (string.IsNullOrWhiteSpace(fileBinding.Activity.OutputFile))
					{
						// generate auto name for file
						fileBinding.Activity.OutputFile = string.Format("{0}.{1}.cs", fileBinding.Activity.NamespaceName, fileBinding.Activity.ClassName);
					}

					// add generated directory
					fileBinding.Activity.OutputFile = PathHelper.Normalize(Path.Combine(DefaultClassLocation, fileBinding.Activity.OutputFile));
					fileBinding.View.InputFile = PathHelper.Normalize(Path.Combine(relativePath, fileBinding.View.InputFile));
					if (string.IsNullOrWhiteSpace(fileBinding.View.OutputFile))
					{
						fileBinding.View.OutputFile = PathHelper.Normalize(Path.Combine(DefaultResourceLocation, fileBinding.View.OutputFile));
					}
				}

				return result;
			}
			catch (Exception)
			{
				BindingPreprocess.Logger.LogError("Cannot read input configuration file {0}", inputFile);
				throw;
			}
		}
	}
}
