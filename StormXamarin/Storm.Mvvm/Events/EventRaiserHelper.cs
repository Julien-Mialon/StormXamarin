using System;

namespace Storm.Mvvm.Events
{
	public static class EventRaiserHelper
	{
		public static void RaiseEvent(this object context, Func<EventHandler> eventGetter)
		{
			context.RaiseEvent(eventGetter());
		}

		public static void RaiseEvent(this object context, Func<EventHandler> eventGetter, EventArgs args)
		{
			context.RaiseEvent(eventGetter(), args);
		}

		public static void RaiseEvent(this object context, EventHandler handler)
		{
			context.RaiseEvent(handler, EventArgs.Empty);
		}

		public static void RaiseEvent(this object context, EventHandler handler, EventArgs args)
		{
			if (handler != null)
			{
				handler(context, args);
			}
		}

		public static void RaiseEvent<T>(this object context, Func<EventHandler<T>> eventGetter, T args)
		{
			context.RaiseEvent(eventGetter(), args);
		}

		public static void RaiseEvent<T>(this object context, EventHandler<T> handler, T args)
		{
			if (handler != null)
			{
				handler(context, args);
			}
		}
	}
}
