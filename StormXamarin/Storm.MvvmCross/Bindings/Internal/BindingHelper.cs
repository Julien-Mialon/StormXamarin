using System;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore;
using Storm.MvvmCross.Bindings.Services;
using Storm.MvvmCross.Interfaces;

namespace Storm.MvvmCross.Bindings.Internal
{
	internal static class BindingHelper
	{
		private static IBindingTypeConverterService _typeConverterService;
		private static IBindingSpecificEventService _specificEventService;
		private static IBindingSpecificPropertyService _specificPropertyService;

		internal static IBindingTypeConverterService TypeConverterService
		{
			get { return _typeConverterService ?? (_typeConverterService = Mvx.Resolve<IBindingTypeConverterService>()); }
		}

		internal static IBindingSpecificEventService SpecificEventService
		{
			get { return _specificEventService ?? (_specificEventService = (Mvx.Resolve<IBindingSpecificEventService>() ?? new DefaultBindingSpecificEventService())); }
		}

		internal static IBindingSpecificPropertyService SpecificPropertyService
		{
			get { return _specificPropertyService ?? (_specificPropertyService = (Mvx.Resolve<IBindingSpecificPropertyService>() ?? new DefaultBindingSpecificPropertyService())); }
		}

		internal static PropertyInfo GetPropertyForBinding(this Type type, string propertyName)
		{
			return type.GetRuntimeProperties().FirstOrDefault(x =>
				string.Equals(propertyName, x.Name, StringComparison.OrdinalIgnoreCase)
				&& (
					(x.GetMethod != null && !x.GetMethod.IsStatic && x.GetMethod.IsPublic) ||
					(x.SetMethod != null && !x.SetMethod.IsStatic && x.SetMethod.IsPublic))
				);
		}

		internal static EventInfo GetEventForBinding(this Type type, string eventName)
		{
			return type.GetRuntimeEvents().FirstOrDefault(x => 
				string.Equals(x.Name, eventName, StringComparison.OrdinalIgnoreCase)
				&& x.AddMethod.IsPublic && !x.AddMethod.IsStatic
				);
		}
	}
}
