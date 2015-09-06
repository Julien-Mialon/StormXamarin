using System;
using Storm.MvvmCross.Android.Wrapper;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Android.Services
{
	public class AndroidBindingTypeConverterService : IBindingTypeConverterService
    {
		public object ConvertToType(object value, Type expectedType)
		{
			return ConverterHelper.ChangeType(value, expectedType);
		}
    }
}
