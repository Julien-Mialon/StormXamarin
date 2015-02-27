using System.Collections.Generic;
using Storm.Binding.AndroidTarget.Configuration.Model;

namespace Storm.Binding.AndroidTarget.Configuration
{
	public static class DefaultConfiguration
	{
		public static IEnumerable<string> Namespaces { get; private set; }

		public static IEnumerable<AliasDescription> Aliases { get; private set; }

		public static IEnumerable<string> CustomAttribute { get; private set; } 

		static DefaultConfiguration()
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

			CustomAttribute = new List<string>
			{
				"CommandParameter",
			};
		}
	}
}
