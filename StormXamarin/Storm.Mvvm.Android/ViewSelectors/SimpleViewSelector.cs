using Android.Views;

namespace Storm.Mvvm.ViewSelectors
{
	public class SimpleViewSelector : ViewSelectorBase
	{
		private readonly int _viewId;

		public SimpleViewSelector(LayoutInflater layoutInflater, int viewId) : base(layoutInflater)
		{
			_viewId = viewId;
		}

		public override int GetViewId(object model)
		{
			return _viewId;
		}
	}
}
