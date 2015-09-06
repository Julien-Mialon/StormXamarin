using System;

namespace Storm.MvvmCross.Interfaces
{
	public interface IBindingTypeConverterService
	{
		object ConvertToType(object value, Type expectedType);
	}
}
