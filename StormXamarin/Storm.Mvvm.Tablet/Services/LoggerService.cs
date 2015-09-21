using System.Diagnostics;

namespace Storm.Mvvm.Services
{
	public class LoggerService : ILoggerService
	{
		public void Log(string message)
		{
			Log(message, MessageSeverity.Debug);
		}

		public void Log(string message, MessageSeverity severity)
		{
			Debug.WriteLine("Log/{0} : {1}", severity, message);
		}
	}
}
