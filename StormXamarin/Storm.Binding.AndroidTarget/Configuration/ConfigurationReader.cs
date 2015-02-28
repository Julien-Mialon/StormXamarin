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
		public ConfigurationFile Read(string inputFile)
		{
			try
			{
				string content = File.ReadAllText(inputFile);

				ConfigurationFile result = JsonConvert.DeserializeObject<ConfigurationFile>(content);

				string relativePath = Path.GetDirectoryName(inputFile) ?? "";
				//Set classLocation & resourceLocation if they are not set into the json file
				result.ClassLocation = result.ClassLocation == null ? DefaultConfiguration.ClassLocation : PathHelper.Normalize(Path.Combine(relativePath, result.ClassLocation));
				result.ResourceLocation = result.ResourceLocation == null ? DefaultConfiguration.ResourceLocation : PathHelper.Normalize(Path.Combine(relativePath, result.ResourceLocation));
				if (string.IsNullOrWhiteSpace(result.GeneratedNamespace))
				{
					result.GeneratedNamespace = DefaultConfiguration.GeneratedNamespace;
				}
				if (string.IsNullOrWhiteSpace(result.DefaultTemplateSelector))
				{
					result.DefaultTemplateSelector = DefaultConfiguration.TemplateSelector;
				}
				if (string.IsNullOrWhiteSpace(result.DefaultTemplateSelectorField))
				{
					result.DefaultTemplateSelectorField = DefaultConfiguration.TemplateSelectorField;
				}
				if (string.IsNullOrWhiteSpace(result.DefaultAdapter))
				{
					result.DefaultAdapter = DefaultConfiguration.Adapter;
				}
				if (string.IsNullOrWhiteSpace(result.DefaultAdapterField))
				{
					result.DefaultAdapterField = DefaultConfiguration.AdapterField;
				}

				//Add all default aliases if they have not been overriden
				Dictionary<string, string> aliases = result.Aliases.ToDictionary(x => x.Alias, x => x.FullClassName);
				foreach (AliasDescription alias in DefaultConfiguration.Aliases.Where(alias => !aliases.ContainsKey(alias.Alias)))
				{
					result.Aliases.Add(alias);
				}

				//Add all default namespaces
				foreach (string name in DefaultConfiguration.Namespaces.Where(name => !result.Namespaces.Contains(name)))
				{
					result.Namespaces.Add(name);
				}
				if (!result.Namespaces.Contains(result.GeneratedNamespace))
				{
					result.Namespaces.Add(result.GeneratedNamespace);
				}
				result.Namespaces.Sort();

				
				//Process files to complete with full path
				foreach (FileBindingDescription fileBinding in result.FileDescriptions)
				{
					if (string.IsNullOrWhiteSpace(fileBinding.Activity.OutputFile))
					{
						// generate auto name for activity file
						fileBinding.Activity.OutputFile = string.Format("{0}.{1}.cs", fileBinding.Activity.NamespaceName, fileBinding.Activity.ClassName);
					}

					if (string.IsNullOrWhiteSpace(fileBinding.View.OutputFile))
					{
						// generate auto name for view file
						fileBinding.View.OutputFile = string.Format("{0}.axml", Path.GetFileNameWithoutExtension(fileBinding.View.InputFile));
					}

					// add full path as prefix to all file
					fileBinding.Activity.OutputFile = PathHelper.Normalize(Path.Combine(result.ClassLocation, fileBinding.Activity.OutputFile));
					fileBinding.View.OutputFile = PathHelper.Normalize(Path.Combine(result.ResourceLocation, fileBinding.View.OutputFile));
					fileBinding.View.InputFile = PathHelper.Normalize(Path.Combine(relativePath, fileBinding.View.InputFile));
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
