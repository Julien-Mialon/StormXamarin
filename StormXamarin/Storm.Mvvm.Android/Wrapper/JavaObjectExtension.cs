using System.Reflection;

namespace Storm.Mvvm.Wrapper
{
	public static class JavaObjectExtension
	{
		public static T ToManaged<T>(this Java.Lang.Object o) where T : class
		{
			return o.ToManaged() as T;
		}

		public static object ToManaged(this Java.Lang.Object o)
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
