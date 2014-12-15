namespace Storm.Mvvm.Services
{
	public class LoggerService : ILoggerService
	{
		public void Log(string message)
		{
			Log(message, MessageSeverity.Info);
		}

		public void Log(string message, MessageSeverity severity)
		{
			
		}
	}
}
