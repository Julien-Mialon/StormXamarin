using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.Dialogs;

namespace Storm.Mvvm.Framework.ColorPicker
{
	public abstract class AbstractColorPickerDialog : AlertDialogFragmentBase
	{
		private uint _oldColor;
		private uint _currentColor;

		private ColorPickerView _pickerView;
		private ColorPanelView _oldPanelView;
		private ColorPanelView _currentPanelView;

		public uint OldColor
		{
			get { return _oldColor; }
			set
			{
				if (SetProperty(ref _oldColor, value))
				{
					InitWithColor(value);
				}
			}
		}

		public uint CurrentColor
		{
			get { return _currentColor; }
			set { SetProperty(ref _currentColor, value); }
		}

		protected AbstractColorPickerDialog()
		{
			Title = "Pick a color";
			Buttons.Add(DialogsButton.Positive, "Ok");
			Buttons.Add(DialogsButton.Negative, "Cancel");
		}

		protected override View CreateView(LayoutInflater inflater, ViewGroup container)
		{
			LinearLayout rootView = new LinearLayout(Activity)
			{
				Orientation = Orientation.Vertical,
			};
			rootView.SetGravity(GravityFlags.Center);
			rootView.SetPaddingRelative(16, 10, 16, 10);

			_pickerView = new ColorPickerView(Activity);
			_oldPanelView = new ColorPanelView(Activity);
			_currentPanelView = new ColorPanelView(Activity);

			TextView textView = new TextView(Activity)
			{
				Text = "→", 
				TextSize = 40, 
				Gravity = GravityFlags.Center
			};
			textView.SetTextColor(Color.White);
			textView.SetPadding(10, 1, 10, 1);
			float density = Application.Context.Resources.DisplayMetrics.Density;

			LinearLayout bottomView = new LinearLayout(Activity)
			{
				Orientation = Orientation.Horizontal
			};
			LinearLayout.LayoutParams layoutparams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, 0.5f);


			rootView.AddView(_pickerView);
			LinearLayout.LayoutParams rootLayoutParams = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, (int) (density*40));
			rootLayoutParams.SetMargins(10, 10, 10, 10);
			rootView.AddView(bottomView, rootLayoutParams);

			bottomView.AddView(_oldPanelView, layoutparams);
			bottomView.AddView(textView);
			bottomView.AddView(_currentPanelView, layoutparams);

			//setting up colors
			_pickerView.OnColorChanged += OnPickerColorChanged;

			_oldPanelView.Color = OldColor;
			_pickerView.setColor((int)OldColor, true);

			return rootView;
		}

		private void OnPickerColorChanged(object sender, int i)
		{
			CurrentColor = (uint) i;
			_currentPanelView.Color = CurrentColor;
		}

		private void InitWithColor(uint color)
		{
			_oldPanelView.Color = color;
			_pickerView.setColor((int)color, true);
		}
	}
}
