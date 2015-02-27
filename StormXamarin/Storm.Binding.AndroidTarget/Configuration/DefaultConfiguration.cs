﻿using System;
using System.Collections.Generic;
using System.IO;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Helper;

namespace Storm.Binding.AndroidTarget.Configuration
{
	public static class DefaultConfiguration
	{
		public static IEnumerable<string> Namespaces { get; private set; }

		public static IEnumerable<AliasDescription> Aliases { get; private set; }

		public static IEnumerable<string> CustomAttribute { get; private set; }

		public static string ClassLocation { get; private set; }

		public static string ResourceLocation { get; private set; }

		public static string GeneratedNamespace { get; private set; }

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
				"Storm.Mvvm.TemplateSelectors",
			};

			CustomAttribute = new List<string>
			{
				"CommandParameter",
			};

			GeneratedNamespace = "Storm.Generated";
			ClassLocation = PathHelper.Normalize(Path.Combine(PathHelper.ProjectDirectory, "Generated.tmp/"));
			ResourceLocation = PathHelper.Normalize(Path.Combine(PathHelper.ProjectDirectory, "Resources/layout/"));
		}
	}
}
