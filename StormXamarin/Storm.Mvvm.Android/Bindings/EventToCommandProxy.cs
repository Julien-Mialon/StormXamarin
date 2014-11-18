using System;
using System.Reflection;
using System.Windows.Input;

namespace Storm.Mvvm.Android.Bindings
{
	public class EventToCommandProxy : EventHookBase
	{
		private static readonly MethodInfo _triggerMethodInfo = null;

		public ICommand Command { get; set; }

		static EventToCommandProxy()
		{
			_triggerMethodInfo = typeof (EventToCommandProxy).GetMethod("OnEventTriggered", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public EventToCommandProxy(object context, EventInfo eventInfo) : base(context, eventInfo, _triggerMethodInfo)
		{
			
		}

		public EventToCommandProxy(object context, EventInfo eventInfo, ICommand command) : this(context, eventInfo)
		{
			Command = command;
		}

		private void OnEventTriggered(object sender, EventArgs e)
		{
			ICommand command = Command;
			if (command != null && command.CanExecute(e))
			{
				command.Execute(e);
			}
		}
	}
}
