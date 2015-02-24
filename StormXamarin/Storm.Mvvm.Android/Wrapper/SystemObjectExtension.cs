using System;
using System.Linq;
using System.Reflection;
using Object = Java.Lang.Object;

namespace Storm.Mvvm.Wrapper
{
	public static class SystemObjectExtension
	{
		private static ConstructorInfo _constructor = null;

		public static Object WrapIntoJava(this object o)
		{
			if (_constructor == null)
			{
				Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith("Mono.Android"));
				if (assembly == null)
				{
					throw new Exception("Could not find assembly Mono.Android");
				}
				TypeInfo javaObjectType = assembly.DefinedTypes.FirstOrDefault(x => x.FullName == "Android.Runtime.JavaObject");
				if (javaObjectType == null)
				{
					throw new Exception("Could not find type Android.Runtime.JavaObject in assembly Mono.Android");
				}
				_constructor = javaObjectType.GetConstructor(new Type[] {typeof (object)});
				if (_constructor == null)
				{
					throw new Exception("Could not find appropriate constructor for type Android.Runtime.JavaObject");
				}
			}
			return _constructor.Invoke(new object[] {o}) as Object;
		}
	}
}
