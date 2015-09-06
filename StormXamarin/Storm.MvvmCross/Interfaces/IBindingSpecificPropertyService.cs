using System.Reflection;

namespace Storm.MvvmCross.Interfaces
{
	public delegate void PropertySetValueAction(PropertyInfo property, object context, object value);

	public interface IBindingSpecificPropertyService
	{
		PropertySetValueAction DetectProperty(object context, PropertyInfo property, PropertySetValueAction defaultSetter);
	}
}
