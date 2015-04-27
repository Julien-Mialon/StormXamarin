using System;
using System.Windows.Data;

namespace Storm.Mvvm.Bindings
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Event)]
	public sealed class BindingAttribute : Attribute
	{
		public string Path { get; set; }

		public Type Converter { get; set; }

		public object ConverterParameter { get; set; }

		//No update event even if Mode=TwoWay, we use the PropertyChanged event of the Activity
		//TODO : Will be also linked to CollectionChanged if needed
		//FIX : to fix 
		public BindingMode Mode { get; set; }

		public BindingAttribute()
		{
			
		}

		public BindingAttribute(string path)
		{
			Path = path;
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
