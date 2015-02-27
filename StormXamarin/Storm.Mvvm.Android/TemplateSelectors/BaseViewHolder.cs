using System.Collections.Generic;
using Android.Views;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm.TemplateSelectors
{
	public abstract class BaseViewHolder
	{
		protected LayoutInflater LayoutInflater { get; private set; }

		protected View View { get; private set; }

		protected object ViewModel { get; private set; }

		protected BaseViewHolder(LayoutInflater layoutInflater, View view)
		{
			LayoutInflater = layoutInflater;
			View = view;
		}

		public void SetViewModel(object model)
		{
			ViewModel = model;
			BindingProcessor.ProcessBinding(ViewModel, this, GetBindingPaths());
		}

		public abstract List<BindingObject> GetBindingPaths();
	}
}
