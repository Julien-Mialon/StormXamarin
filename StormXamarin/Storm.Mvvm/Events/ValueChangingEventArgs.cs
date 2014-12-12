using System;

namespace Storm.Mvvm.Events
{
	public class ValueChangingEventArgs<T> : EventArgs
	{
		public T OldValue { get; private set; }

		public T NewValue { get; private set; }

		public ValueChangingEventArgs(T oldValue, T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}
	}
}
