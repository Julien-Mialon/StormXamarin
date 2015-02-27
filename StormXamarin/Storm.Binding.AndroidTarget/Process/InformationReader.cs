using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Model;
using Storm.Binding.AndroidTarget.Res;

namespace Storm.Binding.AndroidTarget.Process
{
	class InformationReader
	{
		public List<FileBindingDescription> ActivityViewInformations { get; private set; }

		public List<string> AdditionalNamespaces { get; private set; }

		public Dictionary<string, string> ViewComponents { get; private set; } 

		public InformationReader(string filename, string classLocation, string resourceLocation)
		{
			try
			{
				JsonSerializer serializer = new JsonSerializer();

				StreamReader re = new StreamReader(filename);
				JsonTextReader reader = new JsonTextReader(re);

				ConfigurationFile input = serializer.Deserialize<ConfigurationFile>(reader);

				ActivityViewInformations = input.FileDescriptions;
				AdditionalNamespaces = input.Namespaces ?? new List<string>();

				string baseDir = Path.GetDirectoryName(filename) ?? "";
				//rewrite all path using baseDir
				foreach (FileBindingDescription info in ActivityViewInformations)
				{
					info.Activity.OutputFile = NormalizePath(Path.Combine(classLocation, string.Format("{0}.{1}.cs", info.Activity.NamespaceName, info.Activity.ClassName)));
					info.View.InputFile = NormalizePath(Path.Combine(baseDir, info.View.InputFile));
					info.View.OutputFile = NormalizePath(Path.Combine(resourceLocation, info.View.OutputFile));
				}

				List<AliasDescription> components = input.Aliases ?? new List<AliasDescription>(); 
				components.AddRange(DefaultComponents.Components);
				ViewComponents = components.ToDictionary(x => x.Alias, x => x.FullClassName);
			}
			catch (Exception)
			{
				BindingPreprocess.Logger.LogError("Can not read input file {0}", filename);
				throw;
			}
		}

		public static string NormalizePath(string path)
		{
			return Path.GetFullPath(new Uri(Path.GetFullPath(path)).LocalPath)
				.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
		}
	}
}
