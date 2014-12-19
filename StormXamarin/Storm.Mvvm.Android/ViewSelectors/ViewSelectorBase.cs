using System.Collections.Generic;
using Android.Views;
using Java.Util;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm.ViewSelectors
{
	public abstract class ViewSelectorBase : IViewSelector
	{
		protected LayoutInflater LayoutInflater { get; private set; }

		public Dictionary<int, List<BindingObject>> BindingDictionary { get; private set; }

		protected ViewSelectorBase(LayoutInflater layoutInflater)
		{
			LayoutInflater = layoutInflater;
			BindingDictionary = new Dictionary<int, List<BindingObject>>();
		}

		public View GetView(object model, ViewGroup parent, View oldView)
		{
			int newViewId = GetViewId(model);
			int oldViewId = (oldView != null) ? (int)oldView.Tag : -1;

			View resultView;

			if (oldViewId == newViewId)
			{
				resultView = oldView;
			}
			else
			{
				resultView = LayoutInflater.Inflate(newViewId, parent, false);
				resultView.Tag = newViewId;
			}

			AssociateViewWithModel(newViewId, resultView, model);

			return resultView;
		}

		public virtual void AssociateViewWithModel(int viewId, View view, object model)
		{
			
		}

		public abstract int GetViewId(object model);
	}
}
