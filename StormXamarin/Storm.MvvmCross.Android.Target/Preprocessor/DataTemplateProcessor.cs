using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Storm.MvvmCross.Android.Target.CodeGenerator;
using Storm.MvvmCross.Android.Target.Configuration.Model;
using Storm.MvvmCross.Android.Target.Helper;
using Storm.MvvmCross.Android.Target.Model;

namespace Storm.MvvmCross.Android.Target.Preprocessor
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

		public void Process(DataTemplateResource dataTemplate, List<Resource> resources, List<StyleResource> styleResources, List<DataTemplateResource> dataTemplatesResources, ConfigurationFile configurationFile)
		{
			string viewOutputFile = Path.Combine(configurationFile.ResourceLocation, string.Format("{0}.axml", dataTemplate.ViewId));
			string viewOutputRelativePath = PathHelper.GetRelativePath(viewOutputFile);

			Log.LogMessage(MessageImportance.High, "\t\t Preprocessing DataTemplate {0}", dataTemplate.Key);

			// Extract informations from xml
			Tuple<List<XmlAttribute>, List<IdViewObject>> expressionResult = _viewFileProcessor.ExtractExpressions(dataTemplate.ResourceElement);
			List<XmlAttribute> expressionAttributes = expressionResult.Item1;
			List<IdViewObject> viewObjects = expressionResult.Item2;
			List<Resource> localResources = _viewFileProcessor.ExtractResources(dataTemplate.ResourceElement);
			// filter resources to find any dataTemplate in it
			List<DataTemplateResource> localDataTemplatesResources = localResources.Where(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement)).Select(x => new DataTemplateResource(x)).ToList();
			localResources.RemoveAll(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement));
			//filter resources for Style
			List<StyleResource> localStyleResources = resources.Where(x => ParsingHelper.IsStyleTag(x.ResourceElement)).Select(x => new StyleResource(x)).ToList();
			localStyleResources.AddRange(styleResources);
			resources.RemoveAll(x => ParsingHelper.IsStyleTag(x.ResourceElement));

			// now write a file for this data template
			Log.LogMessage(MessageImportance.High, "\t\t\t Generating view file for DataTemplate to {0}", viewOutputRelativePath);
			_viewFileWriter.Write(dataTemplate.ResourceElement, viewOutputFile, localStyleResources);
			ResourceFiles.Add(viewOutputRelativePath);

			

			// assign an id to all data template before processing it
			string viewName = dataTemplate.ViewId;
			foreach (DataTemplateResource localDataTemplate in localDataTemplatesResources)
			{
				localDataTemplate.ViewId = string.Format("{0}_DT_{1}", viewName, localDataTemplate.Key);
				localDataTemplate.ViewHolderClassName = NameGeneratorHelper.GetViewHolderName();
			}

			// process each one
			List<Resource> mergedResources = new List<Resource>(resources);
			mergedResources.AddRange(localResources);
			List<DataTemplateResource> mergedDataTemplatesResources = new List<DataTemplateResource>(dataTemplatesResources);
			mergedDataTemplatesResources.AddRange(localDataTemplatesResources);

			foreach (DataTemplateResource localDataTemplate in localDataTemplatesResources)
			{
				Process(localDataTemplate, mergedResources, localStyleResources, mergedDataTemplatesResources, configurationFile);
			}

			string classOutputFile = Path.Combine(configurationFile.ClassLocation, string.Format("{0}.ui.cs", dataTemplate.ViewHolderClassName));
			string classOutputRelativePath = PathHelper.GetRelativePath(classOutputFile);

			Log.LogMessage(MessageImportance.High, "\t\t\t Generating class file for DataTemplate to {0}", classOutputRelativePath);

			List<Resource> totalResources = new List<Resource>(mergedResources);
			totalResources.AddRange(mergedDataTemplatesResources);

			ViewHolderGenerator generator = new ViewHolderGenerator()
			{
				BaseClassType = "Storm.Mvvm.BaseViewHolder",
				ClassName = dataTemplate.ViewHolderClassName,
				Configuration = configurationFile,
				IsPartialClass = false,
				NamespaceName = configurationFile.GeneratedNamespace,
			};
			generator.Preprocess(expressionAttributes, totalResources, viewObjects);
			generator.Generate(classOutputFile);

			ClassFiles.Add(classOutputRelativePath);
		}
	}
}
