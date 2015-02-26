using System.Collections.Generic;
using Storm.Binding.AndroidTarget.Configuration.Model;
using Storm.Binding.AndroidTarget.Model;

namespace Storm.Binding.AndroidTarget.Res
{
	public static class DefaultComponents
	{
		public static List<AliasDescription> Components { get; private set; }

		static DefaultComponents()
		{
			Components = new List<AliasDescription>
			{
				new AliasDescription("BindableSpinner", "Storm.Mvvm.Aliases.BindableSpinner"),
			};
		}
	}
}
