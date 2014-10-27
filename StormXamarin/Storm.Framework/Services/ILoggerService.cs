using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Framework.Services
{
	public interface ILoggerService
	{
		void Log(string _message);

		void LogFileContent(string _filename, string _fileContent);
	}
}
