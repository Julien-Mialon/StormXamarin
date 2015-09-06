using System;
using System.Reflection;
using Cirrious.CrossCore;
using JetBrains.Annotations;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Bindings.Internal
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
		private readonly PropertySetValueAction _updateValueAction;

		static DependencyPropertyProxy()
		{
			_triggerMethodInfo = typeof(DependencyPropertyProxy).GetRuntimeMethod("OnUpdateEventTriggered", new []{typeof(object), typeof(EventArgs)});
		}

		public DependencyPropertyProxy(object context, PropertyInfo property)
			: base(context, null, null)
		{
			_context = context;
			_property = property;

			_updateValueAction = BindingHelper.SpecificPropertyService.DetectProperty(_context, _property, WriteValue);
		}

		public DependencyPropertyProxy(object context, PropertyInfo property, EventInfo updateEvent)
			: base(context, updateEvent, _triggerMethodInfo)
		{
			_context = context;
			_property = property;

			_updateValueAction = BindingHelper.SpecificPropertyService.DetectProperty(_context, _property, WriteValue);
		}

		public void SetValue(object value)
		{
			if (!_property.PropertyType.IsInstanceOfType(value))
			{
				value = BindingHelper.TypeConverterService.ConvertToType(value, _property.PropertyType);
			}

			if (_property.CanRead)
			{
				object referenceValue = _property.GetValue(_context);
				if (!Equals(referenceValue, value) && _property.CanWrite)
				{
					_updateValueAction(_property, _context, value);
				}
			}
			else if (_property.CanWrite)
			{
				_updateValueAction(_property, _context, value);
			}
		}

		private void WriteValue(PropertyInfo property, object context, object value)
		{
			property.SetValue(context, value);
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
