using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Helper;
using Storm.Binding.AndroidTarget.Model;

namespace Storm.Binding.AndroidTarget.Preprocessor
{
	public class ConfigurationPreprocessor
	{
		private const string GENERATED_NAMESPACE = "Storm.Mvvm.Generated";
		private const string VIEWHOLDER_FORMAT = "AutoGen_ViewHolder_{0}";
		private int _viewHolderCounter;

		public List<string> ClassFiles { get; private set; }
		public List<string> ResourceFiles { get; private set; }

		private TaskLoggingHelper Log { get { return BindingPreprocess.Logger; } }

		public ConfigurationPreprocessor()
		{
			ClassFiles = new List<string>();
			ResourceFiles = new List<string>();
		}

		public void Process(ConfigurationFile configurationFile, string baseDir)
		{
			Dictionary<string, string> aliases = configurationFile.Aliases.ToDictionary(x => x.Alias, x => x.FullClassName);
			ViewFileReader viewFileReader = new ViewFileReader(aliases);
			ViewFileProcessor viewFileProcessor = new ViewFileProcessor();
			ViewFileWriter viewFileWriter = new ViewFileWriter();

			foreach (FileBindingDescription fileBindingDescription in configurationFile.FileDescriptions)
			{
				string viewInputRelativePath = PathHelper.GetRelativePath(baseDir, fileBindingDescription.View.InputFile);
				string viewOutputRelativePath = PathHelper.GetRelativePath(baseDir, fileBindingDescription.View.OutputFile);

				Log.LogMessage(MessageImportance.High, "\t# Preprocessing activity {0}.{1} with view {2}", fileBindingDescription.Activity.NamespaceName, fileBindingDescription.Activity.ClassName, viewInputRelativePath);

				XmlElement rootViewElement = viewFileReader.Read(fileBindingDescription.View.InputFile);
				//Parse expression, Extract resources and simplify the view file
				var expressionParsingResult = viewFileProcessor.ExtractExpressions(rootViewElement);
				List<IdViewObject> viewObjects = expressionParsingResult.Item2;
				List<XmlAttribute> expressionAttributes = expressionParsingResult.Item1;
				List<Resource> resources = viewFileProcessor.ExtractResources(rootViewElement);

				//Write the view file for Android (axml format)
				Log.LogMessage(MessageImportance.High, "\t\t Generating view file {0}", viewOutputRelativePath);
				viewFileWriter.Write(rootViewElement, fileBindingDescription.View.OutputFile);
				ResourceFiles.Add(viewOutputRelativePath);

				//filter resources for DataTemplate
				List<ResourceWithId> dataTemplatesResources = resources.Where(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement)).Select(x => new ResourceWithId(x)).ToList();
				resources.RemoveAll(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement));

				//assign an id to all data template before processing it (could be loop or just unordered things)
				string viewName = Path.GetFileNameWithoutExtension(fileBindingDescription.View.OutputFile);
				foreach (ResourceWithId dataTemplate in dataTemplatesResources)
				{
					dataTemplate.ResourceId = string.Format("{0}__DataTemplate__{1}", viewName, dataTemplate.Key);
				}

				//process each data template
				foreach (ResourceWithId dataTemplate in dataTemplatesResources)
				{
					DataTemplateProcessor dataTemplateProcessor = new DataTemplateProcessor();
					dataTemplateProcessor.Process(dataTemplate, resources, dataTemplatesResources, configurationFile, GENERATED_NAMESPACE);
				}
			}
		}

	}
}
