using Android.Views;

namespace Storm.Mvvm.ViewSelectors
{
	public interface IViewSelector
	{
		View GetView(object model, ViewGroup parent, View oldView);
	}
}