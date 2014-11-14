using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Storm.Binding.Android.Data;
using Storm.Binding.Android.Process;

namespace Storm.Binding.Android
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				Console.WriteLine("Invalid usage, pass json description file as argument to this program");
				Console.ReadKey();
				return;
			}

			foreach (string fileName in args)
			{
				InformationReader reader = new InformationReader(fileName);

				ProcessReader(reader);
			}

			Console.WriteLine("==> Finished");

			Console.ReadKey();
		}

		private static void ProcessReader(InformationReader reader)
		{
			foreach (ActivityViewInfo info in reader.ActivityViewInformations)
			{
				Console.WriteLine("Activity : {3}.{2}\n\tInputFile {0}\n\tOutputFile {1}", info.Activity.InputFile, info.Activity.OutputFile, info.Activity.ClassName, info.Activity.NamespaceName);
				Console.WriteLine("View : \n\tInputFile {0}\n\tOutputFile {1}", info.View.InputFile, info.View.OutputFile);
				Console.WriteLine("");

				ViewFileProcessor processor = new ViewFileProcessor();
				XmlElement root = processor.Read(info.View.InputFile);
				List<XmlAttribute> bindingInformations = processor.ExtractBindingInformations(root);

				processor.Write(root, info.View.OutputFile);

				PartialClassGenerator classGenerator = new PartialClassGenerator();
				classGenerator.Generate(info.Activity, bindingInformations);
			}
		}
	}
}
