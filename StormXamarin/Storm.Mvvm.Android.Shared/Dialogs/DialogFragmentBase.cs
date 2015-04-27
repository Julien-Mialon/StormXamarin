using Android.OS;
using Android.Views;

namespace Storm.Mvvm.Dialogs
{
	public abstract class DialogFragmentBase : AbstractDialogFragmentBase
	{
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			RootView = CreateView(inflater, container);
			return RootView;
		}
	}
}
