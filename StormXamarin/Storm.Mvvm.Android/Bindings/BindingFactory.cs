using System;
using System.Linq;
using System.Reflection;

namespace Storm.Mvvm.Android.Bindings
{
	static class BindingFactory
	{
		public static BindingBase Create(BindingExpression expression, object targetObject)
		{
			MemberInfo memberInfo = targetObject.GetType().GetMember(expression.TargetField, MemberTypes.Event | MemberTypes.Property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).SingleOrDefault();

			if (memberInfo != null)
			{
				BindingBase result;
				if (memberInfo.MemberType == MemberTypes.Event)
				{
					result = new EventBinding(expression, targetObject);
				}
				else if (memberInfo.MemberType == MemberTypes.Property)
				{
					if (expression.Mode == BindingMode.OneWay)
					{
						result = new PropertyBinding(expression, targetObject);
					}
					else if (expression.Mode == BindingMode.TwoWay)
					{
						result = new TwoWayPropertyBinding(expression, targetObject);
					}
					else
					{
						throw new NotImplementedException();
					}
				}
				else
				{
					throw new Exception("BindingFactory : binding to member of type " + memberInfo.MemberType + " is not supported");
				}

				result.Initialize();
				return result;
			}

			throw new Exception("BindingFactory : can not find member " + expression.TargetField + " in object of type " + targetObject.GetType());
		}
	}
}
