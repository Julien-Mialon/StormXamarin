using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Storm.Binding.AndroidTarget
{
	/// <summary>
	/// Standalone tasks which help to transform Resw file to Strings.xml for Android.
	/// </summary>
	public class ReswToStrings : Task
	{
		[Required]
		public ITaskItem[] InputFiles { get; set; }

		[Output]
		public ITaskItem[] GeneratedStrings { get; private set; }

		public override bool Execute()
		{
			Log.LogMessage(MessageImportance.High, "===> Preprocessing resw files <===");
			List<string> generatedFiles = new List<string>();
			foreach (ITaskItem inputFile in InputFiles)
			{
				string filePath = inputFile.ItemSpec;

				string realPath = filePath;
				foreach (string name in inputFile.MetadataNames)
				{
					if ("Link".Equals(name, StringComparison.InvariantCultureIgnoreCase))
					{
						realPath = inputFile.GetMetadata(name);
						break;
					}
				}

				string directory = Path.GetDirectoryName(realPath) ?? "";
				string outputFile = Path.Combine(directory, "Strings.xml");

				Log.LogMessage(MessageImportance.High, "\t=> Processing {0}", realPath);
				GenerateStrings(filePath, outputFile);
				Log.LogMessage(MessageImportance.High, "\t\t=> Generated {0}", outputFile);

				generatedFiles.Add(outputFile);
			}

			GeneratedStrings = generatedFiles.Select(x => (ITaskItem)new TaskItem(x)).ToArray();
			Log.LogMessage(MessageImportance.High, "===> End preprocessing resw files, generate {0} Strings.xml", GeneratedStrings.Length);

			return true;
		}

		private void GenerateStrings(string inputPath, string outputFile)
		{
			XElement rootElement = XElement.Load(inputPath);
			IEnumerable<Tuple<string, string>> items = from dataElement in rootElement.Descendants("data")
													   let key = dataElement.Attribute("name").Value
													   let valueElement = dataElement.Element("value")
													   where valueElement != null
													   let value = valueElement.Value
													   select new Tuple<string, string>(key, value);
			XmlDocument document = new XmlDocument();
			document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", null));
			XmlNode rootNode = document.CreateElement("resources");
			document.AppendChild(rootNode);

			foreach (var pair in items)
			{
				XmlNode elementNode = document.CreateElement("string");
				elementNode.InnerText = pair.Item2;
				XmlAttribute attributeName = document.CreateAttribute("name");
				attributeName.Value = pair.Item1;
				elementNode.Attributes.Append(attributeName);

				rootNode.AppendChild(elementNode);
			}


			document.Save(outputFile);
		}
	}
}
