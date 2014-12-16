using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Storm.Mvvm.Framework.ColorPicker
{
	internal class ColorPanelView : View
	{
		private const float BorderWidthPx = 1;

		private uint _color = 0xFF000000;
		private uint _borderColor = 0xFF6E6E6E;

		private Paint _colorPaint;
		private Paint _borderPaint;

		private RectF _colorRect;
		private RectF _drawingRect;

		public ColorPanelView(Context context)
			: base(context)
		{
			Init();
		}

		public uint Color
		{
			get { return _color; }
			set
			{
				_color = value;
				Invalidate();
			}
		}

		public uint BorderColor
		{
			get { return _borderColor; }
			set
			{
				_borderColor = value;
				Invalidate();
			}
		}

		public ColorPanelView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Init();
		}

		public ColorPanelView(Context context, IAttributeSet attrs, int defStyleAttr)
			: base(context, attrs, defStyleAttr)
		{
			Init();
		}


		private void Init()
		{
			_borderPaint = new Paint();
			_colorPaint = new Paint();
		}


		protected override void OnDraw(Canvas canvas)
		{
			RectF rect = _colorRect;

			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (BorderWidthPx > 0)
			{
				_borderPaint.Color = new Color((int) _borderColor);
				canvas.DrawRect(_drawingRect, _borderPaint);
			}

			_colorPaint.Color = new Color((int) _color);
			canvas.DrawRect(rect, _colorPaint);
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int width = MeasureSpec.GetSize(widthMeasureSpec);
			int height = MeasureSpec.GetSize(heightMeasureSpec);

			SetMeasuredDimension(width, height);
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			_drawingRect = new RectF
			{
				Left = PaddingLeft,
				Right = w - PaddingRight,
				Top = PaddingTop,
				Bottom = h - PaddingBottom
			};

			SetUpColorRect();
		}

		private void SetUpColorRect()
		{
			RectF dRect = _drawingRect;

			float left = dRect.Left + BorderWidthPx;
			float top = dRect.Top + BorderWidthPx;
			float bottom = dRect.Bottom - BorderWidthPx;
			float right = dRect.Right - BorderWidthPx;

			_colorRect = new RectF(left, top, right, bottom);
		}
	}
}