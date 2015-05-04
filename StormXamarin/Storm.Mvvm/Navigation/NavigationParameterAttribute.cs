using System;

namespace Storm.Mvvm.Navigation
{
	[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public sealed class NavigationParameterAttribute : Attribute
	{
		public NavigationParameterAttribute()
		{
			Name = null;
			Mode = NavigationParameterMode.Required;
		}

		public NavigationParameterAttribute(string name)
		{
			Name = name;
			Mode = NavigationParameterMode.Required;
		}

		public string Name { get; set; }

		public NavigationParameterMode Mode { get; set; }
	}
}