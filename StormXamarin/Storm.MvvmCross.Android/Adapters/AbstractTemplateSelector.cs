using System.Collections.Generic;
using Android.Views;
using Storm.MvvmCross.Bindings;

namespace Storm.MvvmCross.Android.Adapters
{
	public abstract class AbstractTemplateSelector : ITemplateSelector
	{
		public Dictionary<int, List<BindingObject>> BindingDictionary { get; private set; }

		protected AbstractTemplateSelector()
		{
			BindingDictionary = new Dictionary<int, List<BindingObject>>();
		}

		public View GetView(object model, ViewGroup parent, View oldView)
		{
			DataTemplate newTemplate = GetTemplate(model);
			int oldViewId = (oldView != null) ? (int)oldView.Tag : -1;

			View resultView;

			if (oldViewId == newTemplate.ViewId)
			{
				resultView = oldView;
			}
			else
			{
				resultView = newTemplate.Inflate(parent);
				resultView.Tag = newTemplate.ViewId;
			}

			newTemplate.AttachToViewModel(resultView, model);

			return resultView;
		}

		public abstract DataTemplate GetTemplate(object model);
	}
}
