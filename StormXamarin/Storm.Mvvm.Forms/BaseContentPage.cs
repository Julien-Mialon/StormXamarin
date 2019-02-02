using Xamarin.Forms;

namespace Storm.Mvvm.Forms
{
	public class BaseContentPage : ContentPage
	{
		protected override void OnAppearing()
		{
			base.OnAppearing();
			if (BindingContext is IViewModelLifecycle viewModelLifecycle)
			{
				viewModelLifecycle.OnResume();
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			if (BindingContext is IViewModelLifecycle viewModelLifecycle)
			{
				viewModelLifecycle.OnPause();
			}
		}
	}
}
