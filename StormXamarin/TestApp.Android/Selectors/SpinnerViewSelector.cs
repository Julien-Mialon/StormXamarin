using Android.Views;
using Android.Widget;
using Storm.Mvvm.ViewSelectors;

namespace TestApp.Android.Selectors
{
	public class SpinnerViewSelector : SimpleViewSelector
	{
		public SpinnerViewSelector(LayoutInflater layoutInflater, int viewId) : base(layoutInflater, viewId)
		{
		}

		public override void AssociateViewWithModel(View view, object model)
		{
			if (view is TextView)
			{
				TextView tw = view as TextView;
				tw.Text = model.ToString();
			}

			base.AssociateViewWithModel(view, model);
		}
	}
}
