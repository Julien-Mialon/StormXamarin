using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.Configuration;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Model;
using Storm.Binding.AndroidTarget.Preprocessor;
using Storm.Binding.AndroidTarget.Process;

namespace Storm.Binding.AndroidTarget
{
	public class BindingPreprocess : Task
	{
		public static TaskLoggingHelper Logger { get; private set; }

		[Required]
		public ITaskItem[] InputFiles { get; set; }

		[Output]
		public ITaskItem[] OutputDirectories { get; private set; }

		[Output]
		public ITaskItem[] GeneratedActivityFiles { get; private set; }

		[Output]
		public ITaskItem[] GeneratedAndroidResource { get; private set; }

		public override bool Execute()
		{
			
			if (InputFiles == null || InputFiles.Length == 0)
			{
				return true;
			}

			Logger = Log;
			List<string> outputDirectories = new List<string>();
			try
			{
				Log.LogMessage(MessageImportance.High, "===> Preprocessing files for Android binding <===");

				ConfigurationReader reader = new ConfigurationReader();
				ConfigurationPreprocessor preprocessor = new ConfigurationPreprocessor();

				foreach (ITaskItem inputFile in InputFiles)
				{
					string filePath = inputFile.ItemSpec;

					Log.LogMessage(MessageImportance.High, "\t=> Preprocessing json file : {0}", inputFile);
					ConfigurationFile file = reader.Read(filePath);

					// check existence of output directory
					foreach (string dir in new[] {file.ClassLocation, file.ResourceLocation})
					{
						if (!Directory.Exists(dir))
						{
							Directory.CreateDirectory(dir);
							outputDirectories.Add(dir);
						}
					}

					preprocessor.Process(file);
				}

				GeneratedActivityFiles = preprocessor.ClassFiles.Select(x => (ITaskItem)new TaskItem(x)).ToArray();
				GeneratedAndroidResource = preprocessor.ResourceFiles.Select(x => (ITaskItem)new TaskItem(x)).ToArray();
				Log.LogMessage(MessageImportance.High, "===> End preprocessing for Android binding, generate {0} class and {1} resources", GeneratedActivityFiles.Length, GeneratedAndroidResource.Length);
			}
			catch (Exception e)
			{
				Log.LogErrorFromException(e, true);
			}
			OutputDirectories = outputDirectories.Select(x => (ITaskItem)new TaskItem(x)).ToArray();
			return !Log.HasLoggedErrors;
		}

		

		private IEnumerable<Tuple<string, FileType>> ProcessReader(InformationReader reader, string projectDir)
		{
			const string GENERATED_NAMESPACE = "Storm.Generated";
			reader.AdditionalNamespaces.Add(GENERATED_NAMESPACE);

			List<Tuple<string, FileType>> result = new List<Tuple<string, FileType>>();
			List<Tuple<ResourceDataTemplate, string, string>> viewHolderClasses = new List<Tuple<ResourceDataTemplate, string, string>>();
			ViewFileProcessor processor = new ViewFileProcessor();
			foreach (FileBindingDescription info in reader.ActivityViewInformations)
			{
				Log.LogMessage(MessageImportance.High, "\t# Generating for activity {0}.{1}", info.Activity.NamespaceName, info.Activity.ClassName);

				XmlElement root = processor.Read(info.View.InputFile, reader.ViewComponents);
				Tuple<List<XmlAttribute>, List<IdViewObject>> tupleResult = processor.ExtractExpressions(root);
				List<XmlAttribute> bindingInformations = tupleResult.Item1;
				List<IdViewObject> views = tupleResult.Item2;
				List<Resource> resourceCollection = ResourceParser.ParseResources(root, info.View).ToList();

				string viewOutputRelativePath = GetRelativePath(projectDir, info.View.OutputFile);
				Log.LogMessage(MessageImportance.High, "\t### Generating view file {0}", viewOutputRelativePath);
				processor.Write(root, info.View.OutputFile);
				result.Add(new Tuple<string, FileType>(viewOutputRelativePath, FileType.Resource));

				// output all DataTemplates files
				string viewName = Path.GetFileNameWithoutExtension(info.View.OutputFile);
				foreach (ResourceDataTemplate dataTemplate in resourceCollection.OfType<ResourceDataTemplate>())
				{
					// generate all datatemplate key before generating data templates
					dataTemplate.ResourceId = viewName + "_DataTemplates_" + dataTemplate.Key;
				}

				foreach (ResourceDataTemplate dataTemplate in resourceCollection.OfType<ResourceDataTemplate>())
				{
					string className = ProcessDataTemplate(dataTemplate, result, projectDir, processor, resourceCollection, reader.AdditionalNamespaces, info.Activity.NamespaceName);
					viewHolderClasses.Add(new Tuple<ResourceDataTemplate, string, string>(dataTemplate, info.Activity.NamespaceName, className));
				}

				AbstractClassGenerator classGenerator;
				if (info.Activity.IsFragment)
				{
					classGenerator = new PartialFragmentClassGenerator(info.Activity.NamespaceName, info.Activity.ClassName);
				}
				else
				{
					classGenerator = new PartialActivityClassGenerator(info.Activity.NamespaceName, info.Activity.ClassName);
				}
				classGenerator.BindingAttributes = bindingInformations;
				classGenerator.ViewElements = views;
				classGenerator.Resources = resourceCollection;
				classGenerator.Namespaces = reader.AdditionalNamespaces;


				//PartialClassGenerator classGenerator = new PartialClassGenerator();

				string classOutputRelativePath = GetRelativePath(projectDir, info.Activity.OutputFile);
				Log.LogMessage(MessageImportance.High, "\t### Generating class file {0}", classOutputRelativePath);
				classGenerator.Generate(info.Activity.OutputFile);
				result.Add(new Tuple<string, FileType>(classOutputRelativePath, FileType.Class));
			}

			//Create static class to process dataTemplate ViewHolders
			ViewHolderFactoryGenerator factoryGenerator = new ViewHolderFactoryGenerator(GENERATED_NAMESPACE)
			{
				Templates = viewHolderClasses,
				Namespaces = reader.AdditionalNamespaces,
			};

			string factoryPath = Path.Combine(ClassLocation, string.Format("{0}.generated.cs", ViewHolderFactoryGenerator.FACTORY_CLASS_NAME));
			string factoryRelativePath = GetRelativePath(projectDir, factoryPath);
			Log.LogMessage(MessageImportance.High, "\t### Generating factory class file {0}", factoryRelativePath);
			factoryGenerator.Generate(factoryPath);
			result.Add(new Tuple<string, FileType>(factoryRelativePath, FileType.Class));




			return result;
		}

		public static void LexLog(string message)
		{
			Logger.LogMessage(MessageImportance.High, "##### ====> {0}", message);
		}

		public static void YaccLog(string message)
		{
			
		}
	}
}