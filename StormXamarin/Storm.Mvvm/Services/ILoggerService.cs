namespace Storm.Mvvm.Services
{
	public enum MessageSeverity
	{
		Info,
		Debug,
		Warning,
		Error,
		Critical
	}

	public interface ILoggerService
	{
		void Log(string message);

		void Log(string message, MessageSeverity severity);
	}
}
