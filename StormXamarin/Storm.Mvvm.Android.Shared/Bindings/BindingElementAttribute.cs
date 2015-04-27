using System;
using System.Windows.Data;

namespace Storm.Mvvm.Bindings
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
	public sealed class BindingElementAttribute : Attribute
	{
		public string Path { get; set; }

		public string TargetPath { get; set; }

		public Type Converter { get; set; }

		public object ConverterParameter { get; set; }

		//No update event even if Mode=TwoWay, we use the PropertyChanged event of the Activity
		//TODO : Will be also linked to CollectionChanged if needed
		//FIX : to fix 
		public BindingMode Mode { get; set; }

		public BindingElementAttribute()
		{
			
		}

		internal IValueConverter GetConverter()
		{
			if (Converter != null)
			{
				return Activator.CreateInstance(Converter) as IValueConverter;
			}
			return null;
		}
	}
}
