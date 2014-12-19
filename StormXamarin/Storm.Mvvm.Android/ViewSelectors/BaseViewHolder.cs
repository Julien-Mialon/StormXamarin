using System.Collections.Generic;
using Android.Views;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm.ViewSelectors
{
	public abstract class BaseViewHolder
	{
		protected LayoutInflater LayoutInflater { get; private set; }

		protected View View { get; private set; }

		protected BaseViewHolder(LayoutInflater layoutInflater, View view)
		{
			LayoutInflater = layoutInflater;
			View = view;
		}

		public abstract List<BindingObject> GetBindingPaths();
	}
}
