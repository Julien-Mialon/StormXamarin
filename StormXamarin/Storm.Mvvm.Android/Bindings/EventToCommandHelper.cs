using System;
using System.Reflection;
using System.Windows.Input;

namespace Storm.Mvvm.Android.Bindings
{
	public class EventToCommandHelper
	{
		public static MethodInfo EventMethodInfo = null;

		public ICommand Command { get; set; }

		static EventToCommandHelper()
		{
			Type type = typeof (EventToCommandHelper);
			EventMethodInfo = type.GetMethod("Trigger", BindingFlags.Instance | BindingFlags.Public);
		}

		public EventToCommandHelper()
		{
			
		}

		public EventToCommandHelper(ICommand command)
		{
			Command = command;
		}

		public void Trigger(object sender, EventArgs e)
		{
			if (Command != null && Command.CanExecute(e))
			{
				Command.Execute(e);
			}
		}
	}
}
