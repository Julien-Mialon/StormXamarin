using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Storm.MvvmCross.Android.Target.Configuration;
using Storm.MvvmCross.Android.Target.Configuration.Model;
using Storm.MvvmCross.Android.Target.Preprocessor;

namespace Storm.MvvmCross.Android.Target
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

		public static void LexLog(string message)
		{
			
		}

		public static void YaccLog(string message)
		{
			
		}
	}
}