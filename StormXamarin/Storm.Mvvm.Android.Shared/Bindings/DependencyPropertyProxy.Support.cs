#if SUPPORT
using System;
using System.Reflection;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Storm.Mvvm.Annotations;
using Storm.Mvvm.Wrapper;

namespace Storm.Mvvm.Bindings
{
	class DependencyPropertyProxy : EventHookBase
	{
		public event EventHandler<DependencyPropertyChangedEventArgs> OnPropertyChanged;

		private static readonly MethodInfo _triggerMethodInfo;
		private static Action<DependencyPropertyProxy, object> _normalUpdateValueAction;
		private static Action<DependencyPropertyProxy, object> _specializedBackgroundUpdateValueAction;

		private readonly object _context;
		private readonly PropertyInfo _property;
		private Action<DependencyPropertyProxy, object> _updateValueAction;

		static DependencyPropertyProxy()
		{
			_triggerMethodInfo = typeof(DependencyPropertyProxy).GetMethod("OnUpdateEventTriggered", BindingFlags.Instance | BindingFlags.NonPublic);
			_normalUpdateValueAction = (context, value) => context.WriteStandardValue(value);
			_specializedBackgroundUpdateValueAction = (context, value) => context.WriteBackgroundValue(value);
		}

		public DependencyPropertyProxy(object context, PropertyInfo property)
			: base(context, null, null)
		{
			_context = context;
			_property = property;
			DetectProperty();
		}

		public DependencyPropertyProxy(object context, PropertyInfo property, EventInfo updateEvent)
			: base(context, updateEvent, _triggerMethodInfo)
		{
			_context = context;
			_property = property;
			DetectProperty();
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
					_updateValueAction(this, value);
				}
			}
			else if (_property.CanWrite)
			{
				_updateValueAction(this, value);
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

		private void DetectProperty()
		{
			View view = _context as View;
			if (view != null)
			{
				if (string.Equals(_property.Name, "Background", StringComparison.InvariantCultureIgnoreCase))
				{
					if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
					{
						_updateValueAction = _specializedBackgroundUpdateValueAction;
					}
				}
			}

			if (_updateValueAction == null)
			{
				_updateValueAction = _normalUpdateValueAction;
			}
		}

		private void WriteStandardValue(object value)
		{
			_property.SetValue(_context, value);
		}

		private void WriteBackgroundValue(object value)
		{
			// ReSharper disable once PossibleNullReferenceException : checked before
			(_context as View).SetBackgroundDrawable(value as Drawable);
		}
	}
}

#endif