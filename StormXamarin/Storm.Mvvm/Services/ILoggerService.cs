namespace Storm.Mvvm.Services
{
	public interface ILoggerService
	{
		void Log(string message);

		void LogFileContent(string filename, string fileContent);
	}
}
