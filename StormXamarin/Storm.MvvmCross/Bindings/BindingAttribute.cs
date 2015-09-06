using System;
using Cirrious.CrossCore.Converters;

namespace Storm.MvvmCross.Bindings
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
	public sealed class BindingAttribute : Attribute
	{
		public string Path { get; set; }

		public string TargetPath { get; set; }

		public Type Converter { get; set; }

		public object ConverterParameter { get; set; }

		public BindingMode Mode { get; set; }

		internal IMvxValueConverter CreateConverter()
		{
			if (Converter != null)
			{
				return Activator.CreateInstance(Converter) as IMvxValueConverter;
			}
			throw new InvalidOperationException("Can not create converter, no Converter set");
		}

	}
}
