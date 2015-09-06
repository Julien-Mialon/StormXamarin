using System.Reflection;
using Java.Lang;

namespace Storm.MvvmCross.Android.Wrapper
{
	public static class JavaObjectExtension
	{
		public static T ToManaged<T>(this Object o) where T : class
		{
			return o.ToManaged() as T;
		}

		public static object ToManaged(this Object o)
		{
			PropertyInfo property = o.GetType().GetProperty("Instance");
			if (property == null)
			{
				return null;
			}
			return property.GetValue(o);
		}
	}
}
