using System;

namespace Storm.Mvvm.Navigation
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class NavigationParameterAttribute : Attribute
	{
		public NavigationParameterAttribute()
		{
			Name = null;
		}

		public NavigationParameterAttribute(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}
}
