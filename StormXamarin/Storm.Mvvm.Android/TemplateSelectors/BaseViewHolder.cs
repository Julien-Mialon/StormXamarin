using System.Collections.Generic;
using Android.Views;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Storm.Mvvm.TemplateSelectors
{
	public abstract class BaseViewHolder
	{
		protected LayoutInflater LayoutInflater { get; private set; }

		protected View View { get; private set; }

		protected object ViewModel { get; private set; }

		protected ILocalizationService LocalizationService { get; private set; }

		//TODO : check if we need the inflater ?
		protected BaseViewHolder(LayoutInflater layoutInflater, View view)
		{
			LayoutInflater = layoutInflater;
			View = view;

			LocalizationService = DependencyService.Container.Resolve<ILocalizationService>();
		}

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
