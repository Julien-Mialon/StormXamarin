using System.Reflection;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Bindings.Services
{
	public class DefaultBindingSpecificPropertyService : IBindingSpecificPropertyService
	{
		public PropertySetValueAction DetectProperty(object context, PropertyInfo property, PropertySetValueAction defaultSetter)
		{
			return defaultSetter;
		}
	}
}
