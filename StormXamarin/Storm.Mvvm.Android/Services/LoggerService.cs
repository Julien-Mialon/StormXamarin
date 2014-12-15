using System;
using System.Collections.Generic;
using System.Text;

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
			switch (severity)
			{
				case MessageSeverity.Info:
					Android.Util.Log.Info("Logger/Info", message);
					break;
				case MessageSeverity.Debug:
					Android.Util.Log.Debug("Logger/Debug", message);
					break;
				case MessageSeverity.Warning:
					Android.Util.Log.Warn("Logger/Warning", message);
					break;
				case MessageSeverity.Error:
					Android.Util.Log.Error("Logger/Error", message);
					break;
				case MessageSeverity.Critical:
					Android.Util.Log.Wtf("Logger/Critical", message);
					break;
			}
		}
	}
}
