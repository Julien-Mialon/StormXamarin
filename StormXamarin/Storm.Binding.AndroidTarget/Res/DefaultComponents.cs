using System.Collections.Generic;
using Storm.Binding.AndroidTarget.Data;

namespace Storm.Binding.AndroidTarget.Res
{
	public static class DefaultComponents
	{
		public static List<ViewComponent> Components { get; private set; }

		static DefaultComponents()
		{
			Components = new List<ViewComponent>
			{
				new ViewComponent("BindableSpinner", "Storm.Mvvm.Components.BindableSpinner"),
			};
		}
	}
}
