using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Storm.Binding.AndroidTarget.Data;

namespace Storm.Binding.AndroidTarget.Process
{
	class InformationReader
	{
		public List<ActivityViewInfo> ActivityViewInformations { get; private set; }

		public List<string> AdditionalNamespaces { get; private set; } 

		public InformationReader(string filename, string classLocation, string resourceLocation)
		{
			try
			{
				JsonSerializer serializer = new JsonSerializer();

				StreamReader re = new StreamReader(filename);
				JsonTextReader reader = new JsonTextReader(re);

				ActivityViewInfoCollection input = serializer.Deserialize<ActivityViewInfoCollection>(reader);

				ActivityViewInformations = input.List;
				AdditionalNamespaces = input.Namespaces ?? new List<string>();

				string baseDir = Path.GetDirectoryName(filename) ?? "";
				//rewrite all path using baseDir
				foreach (ActivityViewInfo info in ActivityViewInformations)
				{
					//TODO : test if baseDir is necessary for *.OutputFile
					info.Activity.OutputFile = NormalizePath(Path.Combine(baseDir, classLocation, info.Activity.OutputFile));
					info.View.InputFile = NormalizePath(Path.Combine(baseDir, info.View.InputFile));
					info.View.OutputFile = NormalizePath(Path.Combine(baseDir, resourceLocation, info.View.OutputFile));
				}
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
