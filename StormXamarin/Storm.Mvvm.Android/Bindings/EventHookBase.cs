using System;
using System.Reflection;

namespace Storm.Mvvm.Bindings
{
	public abstract class EventHookBase
	{
		private readonly EventInfo _event;
		private readonly object _context;

		private readonly Delegate _handler;

		protected EventHookBase(object context, EventInfo eventInfo, MethodInfo handlerInfo)
		{
			_event = eventInfo;
			_context = context;

			if (_event != null && handlerInfo != null)
			{
				_handler = Delegate.CreateDelegate(_event.EventHandlerType, this, handlerInfo);
			}
		}

		public void Attach()
		{
			if (_handler != null)
			{
				_event.GetAddMethod().Invoke(_context, new object[] {_handler});
			}
		}

		public void Remove()
		{
			if (_handler != null)
			{
				_event.GetRemoveMethod().Invoke(_context, new object[] {_handler});
			}
		}
	}
}
