using Android.Graphics;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.ViewSelectors;

namespace TestApp.Android.Selectors
{
	public class SpinnerViewSelector : SimpleViewSelector
	{
		public SpinnerViewSelector(LayoutInflater layoutInflater) : base(layoutInflater)
		{
		}

		public override void AssociateViewWithModel(int viewId, View view, object model)
		{
			if (view is TextView)
			{
				TextView tw = view as TextView;
				tw.Text = model.ToString();
				tw.SetTextColor(new Color(0, 0, 0));
			}

			base.AssociateViewWithModel(viewId, view, model);
		}
	}
}
