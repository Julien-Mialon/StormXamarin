using System;
using Cirrious.CrossCore;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Bindings.Services
{
	public class DefaultBindingTypeConverterService : IBindingTypeConverterService
	{
		public object ConvertToType(object value, Type expectedType)
		{
			if (value.GetType().IsInstanceOfType(expectedType))
			{
				return value;
			}

			return Convert.ChangeType(value, expectedType);
		}
	}
}
