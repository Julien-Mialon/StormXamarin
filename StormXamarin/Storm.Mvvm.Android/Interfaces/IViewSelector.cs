using Android.Views;

namespace Storm.Mvvm.Interfaces
{
	public interface IViewSelector
	{
		View GetView(object model, ViewGroup parent, View oldView);
	}
}