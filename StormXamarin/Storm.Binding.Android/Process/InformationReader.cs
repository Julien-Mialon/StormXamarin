using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Storm.Binding.Android.Data;

namespace Storm.Binding.Android.Process
{
	class InformationReader
	{
		public List<BindingInfo> BindingInformations { get; private set; }

		public InformationReader(string filename)
		{
			JsonSerializer serializer = new JsonSerializer();

			StreamReader re = new StreamReader(filename);
			JsonTextReader reader = new JsonTextReader(re);

			BindingInfoCollection input = serializer.Deserialize<BindingInfoCollection>(reader);

			BindingInformations = input.list;

			string baseDir = Path.GetDirectoryName(filename);

			foreach(BindingInfo info in BindingInformations)
			{

			}
		}
	}
}
