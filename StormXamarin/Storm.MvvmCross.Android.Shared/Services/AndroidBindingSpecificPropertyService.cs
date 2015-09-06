using System;
using System.Reflection;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Android.Services
{
    public class AndroidBindingSpecificPropertyService : IBindingSpecificPropertyService
    {
	    public PropertySetValueAction DetectProperty(object context, PropertyInfo property, PropertySetValueAction defaultSetter)
	    {
			View view = context as View;
			if (view != null)
			{
				if (string.Equals(property.Name, "Background", StringComparison.InvariantCultureIgnoreCase))
				{
					if (Build.VERSION.SdkInt < BuildVersionCodes.JellyBean)
					{
						return WriteSpecificBackgroundValue;
					}
				}
			}

		    return defaultSetter;
	    }

		private void WriteSpecificBackgroundValue(PropertyInfo property, object context, object value)
		{
			View view = context as View;
			if (view == null)
			{
				throw new ArgumentException("Context is not an android View", "context");
			}

			view.SetBackgroundDrawable(value as Drawable);
		}
    }
}
