using System;
using Object = Java.Lang.Object;

namespace Storm.MvvmCross.Android.Wrapper
{
	public static class ConverterHelper
	{
		private static readonly Type _javaObjectType = typeof (Object);
		public static object ChangeType(object value, Type conversionType)
		{
			if (value == null)
			{
				return null;
			}
			Type valueType = value.GetType();
			bool valueIsJava = valueType.IsSubclassOf(_javaObjectType) || valueType == _javaObjectType;
			bool expectedIsJava = conversionType.IsSubclassOf(_javaObjectType) || conversionType == _javaObjectType;

			if (expectedIsJava)
			{
				if (valueIsJava)
				{
					return value;
				}
				return value.WrapIntoJava();
			}
			if (valueIsJava)
			{
				object underlyingObject = (value as Object).ToManaged();
				if (underlyingObject != null)
				{
					value = underlyingObject;
				}
			}

			if (conversionType.IsInstanceOfType(value))
			{
				return value;
			}
			
			return Convert.ChangeType(value, conversionType);
		}
	}
}
