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
	public class ConfigurationPreprocessor
	{
		public List<string> ClassFiles { get; private set; }
		public List<string> ResourceFiles { get; private set; }

		private TaskLoggingHelper Log { get { return BindingPreprocess.Logger; } }

		public ConfigurationPreprocessor()
		{
			ClassFiles = new List<string>();
			ResourceFiles = new List<string>();
		}

		public void Process(ConfigurationFile configurationFile)
		{
			Dictionary<string, string> aliases = configurationFile.Aliases.ToDictionary(x => x.Alias, x => x.FullClassName);
			ViewFileReader viewFileReader = new ViewFileReader(aliases);
			ViewFileProcessor viewFileProcessor = new ViewFileProcessor();
			ViewFileWriter viewFileWriter = new ViewFileWriter();

			DataTemplateProcessor dataTemplateProcessor = new DataTemplateProcessor(viewFileProcessor, viewFileWriter);

			List<Resource> globalResources = new List<Resource>();
			List<StyleResource> globalStyleResources = new List<StyleResource>();
			List<DataTemplateResource> globalDataTemplateResources = new List<DataTemplateResource>();
			foreach (string resourceFile in configurationFile.GlobalResourceFiles)
			{
				string resourceRelativePath = PathHelper.GetRelativePath(resourceFile);

				Log.LogMessage(MessageImportance.High, "\t# Preprocessing resource file {0}", resourceRelativePath);

				List<Resource> resources = viewFileProcessor.ExtractGlobalResources(viewFileReader.Read(resourceFile));
				List<StyleResource> styleResources = resources.Where(x => ParsingHelper.IsStyleTag(x.ResourceElement)).Select(x => new StyleResource(x)).ToList();
				resources.RemoveAll(x => ParsingHelper.IsStyleTag(x.ResourceElement));
				List<DataTemplateResource> dataTemplatesResources = resources.Where(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement)).Select(x => new DataTemplateResource(x)).ToList();
				resources.RemoveAll(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement));

				//assign an id to all data template before processing it (could be loop or just unordered things)
				string viewName = Path.GetFileNameWithoutExtension(resourceFile);
				foreach (DataTemplateResource dataTemplate in dataTemplatesResources)
				{
					dataTemplate.ViewId = string.Format("G_{0}_DT_{1}", viewName, dataTemplate.Key);
					dataTemplate.ViewHolderClassName = NameGeneratorHelper.GetViewHolderName();
				}

				globalResources.AddRange(resources);
				globalStyleResources.AddRange(styleResources);
				globalDataTemplateResources.AddRange(dataTemplatesResources);
			}
			//process each data template
			foreach (DataTemplateResource dataTemplate in globalDataTemplateResources)
			{
				dataTemplateProcessor.Process(dataTemplate, globalResources, globalStyleResources, globalDataTemplateResources, configurationFile);
			}


			foreach (FileBindingDescription fileBindingDescription in configurationFile.FileDescriptions)
			{
				string viewInputRelativePath = PathHelper.GetRelativePath(fileBindingDescription.View.InputFile);
				string viewOutputRelativePath = PathHelper.GetRelativePath(fileBindingDescription.View.OutputFile);

				Log.LogMessage(MessageImportance.High, "\t# Preprocessing activity {0}.{1} with view {2}", fileBindingDescription.Activity.NamespaceName, fileBindingDescription.Activity.ClassName, viewInputRelativePath);

				XmlElement rootViewElement = viewFileReader.Read(fileBindingDescription.View.InputFile);
				//Parse expression, Extract resources and simplify the view file
				var expressionParsingResult = viewFileProcessor.ExtractExpressions(rootViewElement);
				List<IdViewObject> viewObjects = expressionParsingResult.Item2;
				List<XmlAttribute> expressionAttributes = expressionParsingResult.Item1;
				List<Resource> resources = viewFileProcessor.ExtractResources(rootViewElement);
				//filter resources for DataTemplate
				List<DataTemplateResource> dataTemplatesResources = resources.Where(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement)).Select(x => new DataTemplateResource(x)).ToList();
				resources.RemoveAll(x => ParsingHelper.IsDataTemplateTag(x.ResourceElement));
				//filter resources for Style
				List<StyleResource> styleResources = resources.Where(x => ParsingHelper.IsStyleTag(x.ResourceElement)).Select(x => new StyleResource(x)).ToList();
				resources.RemoveAll(x => ParsingHelper.IsStyleTag(x.ResourceElement));


				List<StyleResource> totalStyleResources = new List<StyleResource>(styleResources);
				totalStyleResources.AddRange(globalStyleResources);

				//Write the view file for Android (axml format)
				Log.LogMessage(MessageImportance.High, "\t\t Generating view file {0}", viewOutputRelativePath);
				viewFileWriter.Write(rootViewElement, fileBindingDescription.View.OutputFile, totalStyleResources);
				ResourceFiles.Add(viewOutputRelativePath);


				//assign an id to all data template before processing it (could be loop or just unordered things)
				string viewName = Path.GetFileNameWithoutExtension(fileBindingDescription.View.OutputFile);
				foreach (DataTemplateResource dataTemplate in dataTemplatesResources)
				{
					dataTemplate.ViewId = string.Format("{0}_DT_{1}", viewName, dataTemplate.Key);
					dataTemplate.ViewHolderClassName = NameGeneratorHelper.GetViewHolderName();
				}

				List<Resource> totalResources = new List<Resource>(resources);
				totalResources.AddRange(globalResources);
				List<DataTemplateResource> totalDataTemplateResources = new List<DataTemplateResource>(dataTemplatesResources);
				totalDataTemplateResources.AddRange(globalDataTemplateResources);

				//process each data template
				foreach (DataTemplateResource dataTemplate in dataTemplatesResources)
				{
					dataTemplateProcessor.Process(dataTemplate, totalResources, totalStyleResources, totalDataTemplateResources, configurationFile);
				}

				string classOutputFile = fileBindingDescription.Activity.OutputFile;
				string classOutputRelativePath = PathHelper.GetRelativePath(classOutputFile);

				List<Resource> mergedResources = new List<Resource>(totalResources);
				mergedResources.AddRange(totalDataTemplateResources);
				AbstractBindingHandlerClassGenerator generator;
				if (fileBindingDescription.Activity.IsFragment)
				{
					Log.LogMessage(MessageImportance.High, "\t\t Generating class file for Fragment to {0}", classOutputRelativePath);

					generator = new FragmentGenerator
					{
						BaseClassType = null,
						ClassName = fileBindingDescription.Activity.ClassName,
						Configuration = configurationFile,
						IsPartialClass = true,
						NamespaceName = fileBindingDescription.Activity.NamespaceName,
					};
				}
				else
				{
					Log.LogMessage(MessageImportance.High, "\t\t Generating class file for Activity to {0}", classOutputRelativePath);

					generator = new ActivityGenerator
					{
						BaseClassType = null,
						ClassName = fileBindingDescription.Activity.ClassName,
						Configuration = configurationFile,
						IsPartialClass = true,
						NamespaceName = fileBindingDescription.Activity.NamespaceName,
					};
				}

				generator.Preprocess(expressionAttributes, mergedResources, viewObjects);
				generator.Generate(classOutputFile);

				ClassFiles.Add(classOutputRelativePath);
			}

			ClassFiles.AddRange(dataTemplateProcessor.ClassFiles);
			ResourceFiles.AddRange(dataTemplateProcessor.ResourceFiles);

			CreateDummyClass(configurationFile);
		}

		public void CreateDummyClass(ConfigurationFile file)
		{
			string className = NameGeneratorHelper.GetDummyClassName();
			string namespaceName = file.GeneratedNamespace;

			EmptyGenerator generator = new EmptyGenerator
			{
				ClassName = className,
				IsPartialClass = false,
				NamespaceName = namespaceName,
				BaseClassType = null,
				Configuration = file,
			};
			string outputFile = Path.Combine(file.ClassLocation, string.Format("{0}.{1}.cs", namespaceName, className));
			string outputFileRelativePath = PathHelper.GetRelativePath(outputFile);
			generator.Generate(outputFile);

			ClassFiles.Add(outputFileRelativePath);
		}
	}
}
