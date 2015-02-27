using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Helper;
using Storm.Binding.AndroidTarget.Model;
using Storm.Binding.AndroidTarget.Process;

namespace Storm.Binding.AndroidTarget.Preprocessor
{
	class DataTemplateProcessor
	{
		private readonly ViewFileProcessor _viewFileProcessor;
		private readonly ViewFileWriter _viewFileWriter;

		private TaskLoggingHelper Log { get { return BindingPreprocess.Logger; } }

		public List<string> ClassFiles { get; private set; }

		public List<string> ResourceFiles { get; private set; } 

		public DataTemplateProcessor(ViewFileProcessor viewFileProcessor, ViewFileWriter viewFileWriter)
		{
			_viewFileProcessor = viewFileProcessor;
			_viewFileWriter = viewFileWriter;

			ClassFiles = new List<string>();
			ResourceFiles = new List<string>();
		}

		public void Process(ResourceWithId dataTemplate, List<Resource> resources, List<ResourceWithId> dataTemplatesResources, ConfigurationFile configurationFile)
		{
			string viewOutputFile = Path.Combine(configurationFile.ResourceLocation, string.Format("{0}.axml", dataTemplate.ResourceId));
			string viewOutputRelativePath = PathHelper.GetRelativePath(viewOutputFile);

			Log.LogMessage(MessageImportance.High, "\t\t Preprocessing DataTemplate {0}", dataTemplate.Key);

			// Extract informations from xml
			Tuple<List<XmlAttribute>, List<IdViewObject>> expressionResult = _viewFileProcessor.ExtractExpressions(dataTemplate.ResourceElement);
			List<XmlAttribute> expressionAttributes = expressionResult.Item1;
			List<IdViewObject> viewObjects = expressionResult.Item2;
			List<Resource> localResources = _viewFileProcessor.ExtractResources(dataTemplate.ResourceElement);

			// now write a file for this data template
			Log.LogMessage(MessageImportance.High, "\t\t\t Generating view file for DataTemplate to {0}", viewOutputRelativePath);
			_viewFileWriter.Write(dataTemplate.ResourceElement, viewOutputFile);
			ResourceFiles.Add(viewOutputRelativePath);

			// filter resources to find any dataTemplate in it
			List<ResourceWithId> localDataTemplatesResources = localResources.Where(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement)).Select(x => new ResourceWithId(x)).ToList();
			localResources.RemoveAll(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement));

			// assign an id to all data template before processing it
			string viewName = dataTemplate.ResourceId;
			foreach (ResourceWithId localDataTemplate in localDataTemplatesResources)
			{
				localDataTemplate.ResourceId = string.Format("{0}_DT_{1}", viewName, localDataTemplate.Key);
			}

			// process each one
			List<Resource> mergedResources = new List<Resource>(resources);
			mergedResources.AddRange(localResources);
			List<ResourceWithId> mergedDataTemplatesResources = new List<ResourceWithId>(dataTemplatesResources);
			mergedDataTemplatesResources.AddRange(localDataTemplatesResources);

			foreach (ResourceWithId localDataTemplate in mergedDataTemplatesResources)
			{
				Process(localDataTemplate, mergedResources, mergedDataTemplatesResources, configurationFile);
			}

			//TODO : implement view holder generation
			string className = NameGeneratorHelper.GetViewHolderName();
			string classOutputFile = Path.Combine(configurationFile.ClassLocation, string.Format("{0}.ui.cs", className));
			string classOutputRelativePath = PathHelper.GetRelativePath(classOutputFile);

			Log.LogMessage(MessageImportance.High, "\t\t\t Generating class file for DataTemplate to {0}", classOutputRelativePath);

			ViewHolderClassGenerator viewHolderGenerator = new ViewHolderClassGenerator(configurationFile.GeneratedNamespace, className)
			{
				BindingAttributes = expressionAttributes,
				ViewElements = viewObjects,
				Resources = mergedResources,
				DataTemplateResources = mergedDataTemplatesResources,
				Namespaces = configurationFile.Namespaces,
			};

			viewHolderGenerator.Generate(classOutputFile);
			ClassFiles.Add(classOutputRelativePath);

			//TODO : check if we need to return it ?
			//return className;
		}
	}
}
