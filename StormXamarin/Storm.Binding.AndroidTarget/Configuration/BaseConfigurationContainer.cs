using System.Collections.Generic;
using Storm.Binding.AndroidTarget.Configuration.Model;

namespace Storm.Binding.AndroidTarget.Configuration
{
	public static class BaseConfigurationContainer
	{
		public static IEnumerable<string> Namespaces { get; private set; }

		public static IEnumerable<AliasDescription> Aliases { get; private set; }

		static BaseConfigurationContainer()
		{
			Aliases = new List<AliasDescription>
			{
				new AliasDescription("BindableSpinner", "Storm.Mvvm.Components.BindableSpinner"),
				new AliasDescription("SimpleViewSelector", "Storm.Mvvm.ViewSelectors.SimpleViewSelector"),
			};

			Namespaces = new List<string>
			{
				"System",
				"System.Collections.Generic",
				"System.Reflection",
				"Android.App",
				"Android.Content",
				"Android.Runtime",
				"Android.Views",
				"Android.Widget",
				"Android.OS",
				"Storm.Mvvm",
				"Storm.Mvvm.Bindings",
				"Storm.Mvvm.Components",
				"Storm.Mvvm.ViewSelectors",
			};
		}
	}
}
