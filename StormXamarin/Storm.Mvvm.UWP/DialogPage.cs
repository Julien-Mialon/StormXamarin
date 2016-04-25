using System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Storm.Mvvm.Events;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public class DialogPage : Page, IMvvmDialog
	{
		#region Fields

		private Popup _popup;
		private ViewModelBase _viewModel;
		private readonly Brush _backgroundBrush;

		#endregion

		#region Events

		public event EventHandler Dismissed;

		#endregion

		#region Construction

		public DialogPage(int popupHeight)
		{
			PopupHeight = popupHeight;
			_backgroundBrush = new SolidColorBrush(Color.FromArgb(114, 0, 0, 0));
		}

		#endregion

		#region Properties

		public int PopupHeight
		{
			get { return (int)GetValue(PopupHeightProperty); }
			set { SetValue(PopupHeightProperty, value); }
		}

		public bool CloseOnWindowChange
		{
			get { return (bool)GetValue(CloseOnWindowChangeProperty); }
			set { SetValue(CloseOnWindowChangeProperty, value); }
		}

		public string ParametersKey { get; set; }

		#endregion

		#region Dependency Properties

		public static DependencyProperty PopupHeightProperty =
			DependencyProperty.Register("PopupHeight", typeof(int), typeof(DialogPage), new PropertyMetadata(0));

		public static readonly DependencyProperty CloseOnWindowChangeProperty =
			DependencyProperty.Register("CloseOnWindowChange", typeof(bool), typeof(DialogPage), new PropertyMetadata(true));

		#endregion

		#region Public Methods


		public void Show()
		{
			_popup = new Popup
			{
				IsLightDismissEnabled = false
			};
			_popup.SetValue(Canvas.TopProperty, 0);
			_popup.SetValue(Canvas.LeftProperty, 0);
			_popup.Loaded += OnPopupLoaded;
			_popup.Closed += OnPopupClosed;

			Window.Current.Activated += OnWindowActivated;

			_popup.ChildTransitions = new TransitionCollection
			{
				new PopupThemeTransition()
			};

			Height = PopupHeight;
			Width = Window.Current.Bounds.Width;

			Canvas layoutRoot = new Canvas();
			layoutRoot.SetValue(WidthProperty, Window.Current.Bounds.Width);
			layoutRoot.SetValue(HeightProperty, Window.Current.Bounds.Height);
			layoutRoot.Background = _backgroundBrush;
			layoutRoot.Children.Add(this);
			SetValue(Canvas.TopProperty, (Window.Current.Bounds.Height - PopupHeight) / 2);

			_popup.Child = layoutRoot;
			_popup.IsOpen = true;

			_viewModel = DataContext as ViewModelBase;
			if (_viewModel != null)
			{
				_viewModel.OnNavigatedTo(new NavigationArgs(NavigationArgs.NavigationMode.New), ParametersKey);
			}
		}

		/// <summary>
		/// Closes the Popup.
		/// </summary>
		public void Dismiss()
		{
			_popup.IsOpen = false;
		}

		#endregion

		#region Private Methods

		private void OnPopupClosed(object sender, object e)
		{
			this.RaiseEvent(Dismissed);
			_popup.Child = null;
			Window.Current.Activated -= OnWindowActivated;
			_viewModel.OnNavigatedFrom(new NavigationArgs(NavigationArgs.NavigationMode.Back));
		}

		private void OnPopupLoaded(object sender, RoutedEventArgs e)
		{
			Focus(FocusState.Programmatic);
		}

		private void OnWindowActivated(object sender, WindowActivatedEventArgs e)
		{
			if (CloseOnWindowChange && e.WindowActivationState == CoreWindowActivationState.Deactivated)
			{
				Dismiss();
			}
		}

		#endregion
	}
}
