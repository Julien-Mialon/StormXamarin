using System.Collections.Generic;
using Android.Views;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm.TemplateSelectors
{
	public abstract class BaseViewHolder
	{
		internal LayoutInflater LayoutInflater { get; set; }

		internal View View { get; set; }

		protected object ViewModel { get; private set; }
		
		public void SetViewModel(object model)
		{
			ViewModel = model;
			BindingProcessor.ProcessBinding(ViewModel, this, GetBindingPaths());
		}

		public virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}
	}
}
