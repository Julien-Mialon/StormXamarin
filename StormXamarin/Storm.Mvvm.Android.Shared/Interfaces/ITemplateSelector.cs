using Android.Views;

namespace Storm.Mvvm.Interfaces
{
	public interface ITemplateSelector
	{
		View GetView(object model, ViewGroup parent, View oldView);
	}
}