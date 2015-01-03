using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Storm.Binding.AndroidTarget.Data;
using Storm.Binding.AndroidTarget.Process;

namespace Storm.Binding.AndroidTarget
{
	public class BindingPreprocess : Task
	{
		private const string VIEWHOLDER_FORMAT = "AutoGen_ViewHolder_{0}";
		private int _viewHolderCounter;

		public enum FileType
		{
			Class,
			Resource
		}

		public static TaskLoggingHelper Logger { get; private set; }

		[Required]
		public ITaskItem[] InputFiles { get; set; }

		[Required]
		public string ClassLocation { get; set; }

		[Required]
		public string ResourceLocation { get; set; }

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

			Logger = this.Log;
			try
			{
				Log.LogMessage(MessageImportance.High, "===> Preprocessing files for Android binding <===");

				string projectDir = InformationReader.NormalizePath(Environment.CurrentDirectory) + Path.DirectorySeparatorChar;
				List<Tuple<string, FileType>> resultFiles = new List<Tuple<string, FileType>>();
				foreach (ITaskItem file in InputFiles)
				{
					Log.LogMessage(MessageImportance.High, "\t=> Preprocessing json file : {0}", file.ItemSpec);
					string filePath = file.ItemSpec;
					InformationReader reader = new InformationReader(filePath, ClassLocation, ResourceLocation);
					resultFiles.AddRange(ProcessReader(reader, projectDir));
				}

				GeneratedActivityFiles = resultFiles.Where(x => x.Item2 == FileType.Class).Select(x => (ITaskItem)new TaskItem(x.Item1)).ToArray();
				GeneratedAndroidResource = resultFiles.Where(x => x.Item2 == FileType.Resource).Select(x => (ITaskItem)new TaskItem(x.Item1)).ToArray();

				Log.LogMessage(MessageImportance.High, "===> End preprocessing for Android binding, generate {0} class and {1} resources", GeneratedActivityFiles.Length, GeneratedAndroidResource.Length);
			}
			catch (Exception e)
			{
				Log.LogErrorFromException(e, true);
				return false;
			}
			
			return !Log.HasLoggedErrors;
		}

		private string GetRelativePath(string projectDir, string file)
		{
			string normalized = InformationReader.NormalizePath(file);
			if (normalized.StartsWith(projectDir, StringComparison.OrdinalIgnoreCase))
			{
				return file.Substring(projectDir.Length);
			}
			Log.LogError("Error, unable to get relative path from {0} compare to {1}", projectDir, file);
			throw new Exception();
		}

		private IEnumerable<Tuple<string, FileType>> ProcessReader(InformationReader reader, string projectDir)
		{
			const string GENERATED_NAMESPACE = "Storm.Generated";
			reader.AdditionalNamespaces.Add(GENERATED_NAMESPACE);

			List<Tuple<string, FileType>> result = new List<Tuple<string, FileType>>();
			List<Tuple<ResourceDataTemplate, string, string>> viewHolderClasses = new List<Tuple<ResourceDataTemplate, string, string>>();
			ViewFileProcessor processor = new ViewFileProcessor();
			foreach (ActivityViewInfo info in reader.ActivityViewInformations)
			{
				Log.LogMessage(MessageImportance.High, "\t# Generating for activity {0}.{1}", info.Activity.NamespaceName, info.Activity.ClassName);

				XmlElement root = processor.Read(info.View.InputFile, reader.ViewComponents);
				Tuple<List<XmlAttribute>, List<IdViewObject>> tupleResult = processor.ExtractBindingInformations(root);
				List<XmlAttribute> bindingInformations = tupleResult.Item1;
				List<IdViewObject> views = tupleResult.Item2;
				List<XmlResource> resourceCollection = ResourceParser.ParseResources(root, info.View).ToList();

				string viewOutputRelativePath = GetRelativePath(projectDir, info.View.OutputFile);
				Log.LogMessage(MessageImportance.High, "\t### Generating view file {0}", viewOutputRelativePath);
				processor.Write(root, info.View.OutputFile);
				result.Add(new Tuple<string, FileType>(viewOutputRelativePath, FileType.Resource));

				// output all DataTemplates files
				string viewName = Path.GetFileNameWithoutExtension(info.View.OutputFile);
				foreach (ResourceDataTemplate dataTemplate in resourceCollection.OfType<ResourceDataTemplate>())
				{
					dataTemplate.ResourceId = viewName + "_DataTemplates_" + dataTemplate.Key;
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

		/// <summary>
		/// 
		/// </summary>
		/// <returns>The name of the ViewHolder class created for this DataTemplate</returns>
		private string ProcessDataTemplate(ResourceDataTemplate dataTemplate, List<Tuple<string, FileType>> result, string projectDir, ViewFileProcessor processor, IEnumerable<XmlResource> resourceCollection, IEnumerable<string> additionalNamespaces, string namespaceName)
		{
			Tuple<List<XmlAttribute>, List<IdViewObject>> tupleResult = processor.ExtractBindingInformations(dataTemplate.RootElement);
			List<XmlAttribute> bindingInformations = tupleResult.Item1;
			List<IdViewObject> views = tupleResult.Item2;

			string viewOutputFile = Path.Combine(ResourceLocation, dataTemplate.ResourceId + ".axml");
			string viewOutputRelativeFile = GetRelativePath(projectDir, viewOutputFile);
			Log.LogMessage(MessageImportance.High, "\t\t### Generating view for DataTemplate {0} to file {1}", dataTemplate.Key, viewOutputRelativeFile);
			processor.Write(dataTemplate.RootElement, viewOutputFile);

			result.Add(new Tuple<string, FileType>(viewOutputRelativeFile, FileType.Resource));

			string viewHolderClassName = string.Format(VIEWHOLDER_FORMAT, _viewHolderCounter++);
			ViewHolderClassGenerator viewHolderGenerator = new ViewHolderClassGenerator(namespaceName, viewHolderClassName)
			{
				BindingAttributes = bindingInformations, 
				ViewElements = views, 
				Resources = resourceCollection, 
				Namespaces = additionalNamespaces
			};
			string classOutputFile = Path.Combine(ClassLocation, viewHolderClassName + ".ui.cs");
			string classRelativePath = GetRelativePath(projectDir, classOutputFile);
			Log.LogMessage(MessageImportance.High, "\t\t### Generating class for DataTemplate {0} to file {1}", dataTemplate.Key, classRelativePath);
			viewHolderGenerator.Generate(classOutputFile);

			result.Add(new Tuple<string, FileType>(classRelativePath, FileType.Class));

			return viewHolderClassName;
		}
	}
}