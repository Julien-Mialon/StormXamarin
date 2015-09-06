using System;

namespace Storm.MvvmCross.Bindings.Internal
{
	class DependencyPropertyChangedEventArgs : EventArgs
	{
		public string PropertyName { get; private set; }

		public object NewValue { get; private set; }

		public DependencyPropertyChangedEventArgs()
		{

		}

		public DependencyPropertyChangedEventArgs(string propertyName, object newValue)
		{
			PropertyName = propertyName;
			NewValue = newValue;
		}
	}
}