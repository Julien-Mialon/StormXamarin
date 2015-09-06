using Android.Views;

namespace Storm.MvvmCross.Android.Adapters
{
	public interface ITemplateSelector
	{
		View GetView(object model, ViewGroup parent, View oldView);
	}
}