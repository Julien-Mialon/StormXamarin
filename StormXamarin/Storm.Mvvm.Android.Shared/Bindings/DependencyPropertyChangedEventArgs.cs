using System;

namespace Storm.Mvvm.Bindings
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