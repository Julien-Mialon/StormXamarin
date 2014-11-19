namespace Storm.Framework.Services
{
	public interface ILoggerService
	{
		void Log(string _message);

		void LogFileContent(string _filename, string _fileContent);
	}
}
