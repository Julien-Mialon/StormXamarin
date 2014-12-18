using Android.Views;

namespace Storm.Mvvm.ViewSelectors
{
	public abstract class ViewSelectorBase : IViewSelector
	{
		protected LayoutInflater LayoutInflater { get; private set; }

		protected ViewSelectorBase(LayoutInflater layoutInflater)
		{
			LayoutInflater = layoutInflater;
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
