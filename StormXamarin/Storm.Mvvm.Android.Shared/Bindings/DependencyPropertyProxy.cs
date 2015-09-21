#if !SUPPORT
using System;
using System.Reflection;
using Storm.Mvvm.Annotations;
using Storm.Mvvm.Wrapper;

namespace Storm.Mvvm.Bindings
{
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

		public DependencyPropertyProxy(object context, PropertyInfo property)
			: base(context, null, null)
		{
			_context = context;
			_property = property;
		}

		public DependencyPropertyProxy(object context, PropertyInfo property, EventInfo updateEvent)
			: base(context, updateEvent, _triggerMethodInfo)
		{
			_context = context;
			_property = property;
		}

		public void SetValue(object value)
		{
			if (!_property.PropertyType.IsInstanceOfType(value))
			{
				value = ConverterHelper.ChangeType(value, _property.PropertyType);
			}

			if (_property.CanRead)
			{
				object referenceValue = _property.GetValue(_context);
				if (!Equals(referenceValue, value) && _property.CanWrite)
				{
					_property.SetValue(_context, value);
				}
			}
			else if (_property.CanWrite)
			{
				_property.SetValue(_context, value);
			}
		}

		[UsedImplicitly]
		// ReSharper disable once UnusedMember.Local
		// ReSharper disable UnusedParameter.Local
		private void OnUpdateEventTriggered(object sender, EventArgs e)
		// ReSharper restore UnusedParameter.Local
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

#endif