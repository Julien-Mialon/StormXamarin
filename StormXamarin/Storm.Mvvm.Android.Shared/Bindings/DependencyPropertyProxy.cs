using System;
using System.Reflection;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Storm.Mvvm.Annotations;
using Storm.Mvvm.Wrapper;

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
#if SUPPORT
					WriteValue(value);
#else
					_property.SetValue(_context, value);
#endif
				}
			}
			else if (_property.CanWrite)
			{
#if SUPPORT
				WriteValue(value);
#else
				_property.SetValue(_context, value);
#endif
			}
		}

#if SUPPORT
		private void WriteValue(object value)
		{
			// This method intercept write value to property to handle background problem (need to call method SetBackgroundDrawable instead of Background = if api >= 16)
			View view = _context as View;
			if (view != null)
			{
				if (string.Equals(_property.Name, "Background", StringComparison.InvariantCultureIgnoreCase))
				{
					if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
					{
						view.SetBackgroundDrawable(value as Drawable);
						return;
					}
				}
			}

			// if not affected, use the normal method
			_property.SetValue(_context, value);
		}
#endif

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
