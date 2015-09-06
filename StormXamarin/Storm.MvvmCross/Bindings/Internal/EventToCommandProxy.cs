using System;
using System.Reflection;
using System.Windows.Input;
using JetBrains.Annotations;

namespace Storm.MvvmCross.Bindings.Internal
{
	public class EventToCommandProxy : EventHookBase
	{
		private static readonly MethodInfo _triggerMethodInfo;

		public ICommand Command { get; set; }

		public CommandParameterProxy CommandParameter { get; set; }

		static EventToCommandProxy()
		{
			_triggerMethodInfo = typeof (EventToCommandProxy).GetRuntimeMethod("OnEventTriggered", new[] {typeof(object), typeof(EventArgs)});

			if (_triggerMethodInfo == null)
			{
				throw new InvalidOperationException("Can not get method OnEventTriggered of type EventToCommandProxy, did the method disapear because of linking ?");
			}
		}

		public EventToCommandProxy(object context, EventInfo eventInfo) : base(context, eventInfo, _triggerMethodInfo)
		{
			OnEventTriggered(null, null);
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
		private void OnEventTriggered(object sender, EventArgs e)
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
