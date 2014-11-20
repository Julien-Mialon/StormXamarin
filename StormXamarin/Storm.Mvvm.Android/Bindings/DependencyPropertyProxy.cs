using System;
using System.Reflection;
using Storm.Mvvm.Annotations;

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

	/// <summary>
	/// Goal of this class is to act just as a "standard" xaml DependencyProperty
	///		- Provide event when property changed
	///		- Set the property only if it changed
	/// </summary>
	class DependencyPropertyProxy : EventHookBase
	{
		public event EventHandler<DependencyPropertyChangedEventArgs> OnPropertyChanged;

		private static readonly MethodInfo _triggerMethodInfo;

		private readonly object _context;
		private readonly PropertyInfo _property;

		static DependencyPropertyProxy()
		{
			_triggerMethodInfo = typeof(DependencyPropertyProxy).GetMethod("OnUpdateEventTriggered", BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public DependencyPropertyProxy(object context, PropertyInfo property) : base(context, null, null)
		{
			_context = context;
			_property = property;
		}

		public DependencyPropertyProxy(object context, PropertyInfo property, EventInfo updateEvent) : base(context, updateEvent, _triggerMethodInfo)
		{
			_context = context;
			_property = property;
		}

		public void SetValue(object value)
		{
			object referenceValue = _property.GetValue(_context);

			value = Convert.ChangeType(value, _property.PropertyType);
			if (!Equals(referenceValue, value))
			{
				_property.SetValue(_context, value);
			}
		}

		[UsedImplicitly]
		private void OnUpdateEventTriggered(object sender, EventArgs e)
		{
			//Event raised mean property changed so : 
			// - Get the property value
			// - Raise event for this property changed

			NotifyPropertyChanged();
		}

		protected void NotifyPropertyChanged()
		{
			EventHandler<DependencyPropertyChangedEventArgs> handler = OnPropertyChanged;
			if (handler != null)
			{
				handler(this, new DependencyPropertyChangedEventArgs(_property.Name, _property.GetValue(_context)));
			}
		}
	}
}
