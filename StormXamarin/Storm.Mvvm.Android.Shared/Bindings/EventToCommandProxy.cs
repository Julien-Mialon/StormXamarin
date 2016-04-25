using System;
using System.Reflection;
using System.Windows.Input;
using Storm.Mvvm.Annotations;

namespace Storm.Mvvm.Bindings
{
	public class EventToCommandProxy : EventHookBase
	{
		private static readonly MethodInfo _triggerMethodInfo;

		public ICommand Command { get; set; }

		public CommandParameterProxy CommandParameter { get; set; }

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

		public EventToCommandProxy(object context, EventInfo eventInfo, CommandParameterProxy commandParameter) : this(context, eventInfo)
		{
			CommandParameter = commandParameter;
		}

		public EventToCommandProxy(object context, EventInfo eventInfo, ICommand command, CommandParameterProxy commandParameter) : this(context, eventInfo)
		{
			Command = command;
			CommandParameter = commandParameter;
		}

		[UsedImplicitly]
		// ReSharper disable once UnusedMember.Local
		// ReSharper disable UnusedParameter.Local
		private void OnEventTriggered(object sender, EventArgs e)
		// ReSharper restore UnusedParameter.Local
		{
			ICommand command = Command;
			object parameter = CommandParameter == null ? e : CommandParameter.Value;
			if (command != null && command.CanExecute(parameter))
			{
				command.Execute(parameter);
			}
		}
	}
}
