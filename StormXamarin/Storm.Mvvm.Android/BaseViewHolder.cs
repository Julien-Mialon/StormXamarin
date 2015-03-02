using System.Collections.Generic;
using Android.Views;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm
{
	public abstract class BaseViewHolder
	{
		protected internal LayoutInflater LayoutInflater { get; set; }

		protected internal View View { get; set; }

		protected object ViewModel { get; private set; }
		
		public void SetViewModel(object model)
		{
			ViewModel = model;
			BindingProcessor.ProcessBinding(ViewModel, this, GetBindingPaths());
		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}
	}
}
