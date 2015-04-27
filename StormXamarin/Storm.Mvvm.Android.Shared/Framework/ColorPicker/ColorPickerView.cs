﻿using System;
using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace Storm.Mvvm.Framework.ColorPicker
{
	internal class ColorPickerView : View
	{
		public event EventHandler<int>  OnColorChanged;

		private const int PANEL_SAT_VAL = 0;
		private const int PANEL_HUE = 1;
		private const int PANEL_ALPHA = 2;
		private const float BORDER_WIDTH_PX = 1;
		private float HUE_PANEL_WIDTH = 30f;
		private float ALPHA_PANEL_HEIGHT = 20f;
		private float PANEL_SPACING = 10f;
		private float PALETTE_CIRCLE_TRACKER_RADIUS = 5f;
		private float RECTANGLE_TRACKER_OFFSET = 2f;


		private static float mDensity = 1f;

		private Paint mSatValPaint;
		private Paint mSatValTrackerPaint;

		private Paint mHuePaint;
		private Paint mHueAlphaTrackerPaint;

		private Paint mAlphaTextPaint;

		private Paint mBorderPaint;

		private Shader mValShader;
		private Shader mSatShader;
		private Shader mHueShader;


		/*
		 * We cache a bitmap of the sat/val panel which is expensive to draw each time.
		 * We can reuse it when the user is sliding the circle picker as long as the hue isn't changed.
		 */
		private BitmapCache mSatValBackgroundCache;


		private int mAlpha = 0xff;
		private float mHue = 360f;
		private float mSat = 0f;
		private float mVal = 0f;

		private String mAlphaSliderText = null;
		private uint mSliderTrackerColor = 0xFFBDBDBD;
		private uint mBorderColor = 0xFF6E6E6E;
		private bool mShowAlphaPanel = false;

		/*
		 * To remember which panel that has the "focus" when 
		 * processing hardware button data.
		 */
		private int mLastTouchedPanel = PANEL_SAT_VAL;

		/**
		 * Offset from the edge we must have or else
		 * the finger tracker will get clipped when
		 * it is drawn outside of the view.
		 */
		private int mDrawingOffset;


		/*
		 * Distance form the edges of the view 
		 * of where we are allowed to draw.
		 */
		private RectF mDrawingRect;

		private RectF mSatValRect;
		private RectF mHueRect;
		private RectF mAlphaRect;


		private Point mStartTouchPoint = null;


		public ColorPickerView(Context context)
			: base(context)
		{
			init(null);
		}

		public ColorPickerView(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			init(attrs);
		}

		public ColorPickerView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs)
		{
			init(attrs);
		}

		protected void RaiseColorChanged(int newColor)
		{
			EventHandler<int> handler = OnColorChanged;
			if (handler != null)
			{
				handler(this, newColor);
			}
		}

		private void init(IAttributeSet attrs)
		{

			mShowAlphaPanel = false;
			mAlphaSliderText = "";
			mSliderTrackerColor = 0xFFBDBDBD;
			mBorderColor = 0xFF6E6E6E;


			mDensity = Context.Resources.DisplayMetrics.Density;
			PALETTE_CIRCLE_TRACKER_RADIUS *= mDensity;
			RECTANGLE_TRACKER_OFFSET *= mDensity;
			HUE_PANEL_WIDTH *= mDensity;
			ALPHA_PANEL_HEIGHT *= mDensity;
			PANEL_SPACING = PANEL_SPACING * mDensity;

			mDrawingOffset = calculateRequiredOffset();

			initPaintTools();

			Focusable = true;
			FocusableInTouchMode = true;
		}

		private void initPaintTools()
		{

			mSatValPaint = new Paint();
			mSatValTrackerPaint = new Paint();
			mHuePaint = new Paint();
			mHueAlphaTrackerPaint = new Paint();
			new Paint();
			mAlphaTextPaint = new Paint();
			mBorderPaint = new Paint();


			mSatValTrackerPaint.SetStyle(Paint.Style.Stroke);
			mSatValTrackerPaint.StrokeWidth = 2f * mDensity;
			mSatValTrackerPaint.AntiAlias = true;

			mHueAlphaTrackerPaint.Color = new Color((int)mSliderTrackerColor);
			mHueAlphaTrackerPaint.SetStyle(Paint.Style.Stroke);
			mHueAlphaTrackerPaint.StrokeWidth = 2f * mDensity;
			mHueAlphaTrackerPaint.AntiAlias = true;

			uint colorValue = 0xFF1C1C1C;
			mAlphaTextPaint.Color = new Color((int)colorValue);
			mAlphaTextPaint.TextSize = 14f * mDensity;
			mAlphaTextPaint.AntiAlias = true;
			mAlphaTextPaint.TextAlign = Paint.Align.Center;
			mAlphaTextPaint.FakeBoldText = true;

		}

		private int calculateRequiredOffset()
		{
			float offset = Math.Max(PALETTE_CIRCLE_TRACKER_RADIUS, RECTANGLE_TRACKER_OFFSET);
			offset = Math.Max(offset, BORDER_WIDTH_PX * mDensity);

			return (int)(offset * 1.5f);
		}

		private int[] buildHueColorArray()
		{
			int[] hue = new int[361];

			int count = 0;
			for (int i = hue.Length - 1; i >= 0; i--, count++)
			{
				hue[count] = Color.HSVToColor(new float[] { i, 1f, 1f });
			}

			return hue;
		}


		protected override void OnDraw(Canvas canvas)
		{
			if (mDrawingRect.Width() <= 0 || mDrawingRect.Height() <= 0)
			{
				return;
			}

			drawSatValPanel(canvas);
			drawHuePanel(canvas);
			//drawAlphaPanel(canvas);

		}

		private void drawSatValPanel(Canvas canvas)
		{
			/*
			 * Draw time for this code without using bitmap cache and hardware acceleration was around 20ms.
			 * Now with the bitmap cache and the ability to use hardware acceleration we are down at 1ms as long as the hue isn't changed.
			 * If the hue is changed we the sat/val rectangle will be rendered in software and it takes around 10ms.
			 * But since the rest of the view will be rendered in hardware the performance gain is big!
			 */

			RectF rect = mSatValRect;

			if (BORDER_WIDTH_PX > 0)
			{
				mBorderPaint.Color = new Color((int)mBorderColor);
				canvas.DrawRect(mDrawingRect.Left, mDrawingRect.Top, rect.Right + BORDER_WIDTH_PX, rect.Bottom + BORDER_WIDTH_PX, mBorderPaint);
			}

			if (mValShader == null)
			{
				//Black gradient has either not been created or the view has been resized.			
				uint startColor = 0xFFFFFFFF;
				uint endColor = 0xFF000000;
				mValShader = new LinearGradient(rect.Left, rect.Top, rect.Left, rect.Bottom,
						new Color((int)startColor), new Color((int)endColor), Shader.TileMode.Clamp);
			}


			//If the hue has changed we need to recreate the cache.
			if (mSatValBackgroundCache == null || mSatValBackgroundCache.value != mHue)
			{

				if (mSatValBackgroundCache == null)
				{
					mSatValBackgroundCache = new BitmapCache();
				}

				//We create our bitmap in the cache if it doesn't exist.
				if (mSatValBackgroundCache.bitmap == null)
				{
					mSatValBackgroundCache.bitmap = Bitmap.CreateBitmap((int)rect.Width(), (int)rect.Height(), Bitmap.Config.Argb8888);
				}

				//We create the canvas once so we can draw on our bitmap and the hold on to it.
				if (mSatValBackgroundCache.canvas == null)
				{
					mSatValBackgroundCache.canvas = new Canvas(mSatValBackgroundCache.bitmap);
				}

				int rgb = Color.HSVToColor(new float[] { mHue, 1f, 1f });
				uint startColor = 0xFFFFFFFF;
				mSatShader = new LinearGradient(rect.Left, rect.Top, rect.Right, rect.Top,
						new Color((int)startColor), new Color(rgb), Shader.TileMode.Clamp);

				ComposeShader mShader = new ComposeShader(mValShader, mSatShader, PorterDuff.Mode.Multiply);
				mSatValPaint.SetShader(mShader);

				//ly we draw on our canvas, the result will be stored in our bitmap which is already in the cache.
				//Since this is drawn on a canvas not rendered on screen it will automatically not be using the hardware acceleration.
				//And this was the code that wasn't supported by hardware acceleration which mean there is no need to turn it of anymore.
				//The rest of the view will still be hardware accelerated!!
				mSatValBackgroundCache.canvas.DrawRect(0, 0, mSatValBackgroundCache.bitmap.Width, mSatValBackgroundCache.bitmap.Height, mSatValPaint);

				//We set the hue value in our cache to which hue it was drawn with, 
				//then we know that if it hasn't changed we can reuse our cached bitmap.
				mSatValBackgroundCache.value = mHue;

			}

			//We draw our bitmap from the cached, if the hue has changed
			//then it was just recreated otherwise the old one will be used.
			canvas.DrawBitmap(mSatValBackgroundCache.bitmap, null, rect, null);


			Point p = satValToPoint(mSat, mVal);

			mSatValTrackerPaint.Color = Color.Black;
			canvas.DrawCircle(p.X, p.Y, PALETTE_CIRCLE_TRACKER_RADIUS - 1f * mDensity, mSatValTrackerPaint);

			mSatValTrackerPaint.Color = Color.LightGray;
			canvas.DrawCircle(p.X, p.Y, PALETTE_CIRCLE_TRACKER_RADIUS, mSatValTrackerPaint);

		}

		private void drawHuePanel(Canvas canvas)
		{
			/*
			 * Drawn with hw acceleration, very fast.
			 */

			//long start = SystemClock.elapsedRealtime();

			RectF rect = mHueRect;

			if (BORDER_WIDTH_PX > 0)
			{
				mBorderPaint.Color = new Color((int)mBorderColor);
				canvas.DrawRect(rect.Left - BORDER_WIDTH_PX,
						rect.Top - BORDER_WIDTH_PX,
						rect.Right + BORDER_WIDTH_PX,
						rect.Bottom + BORDER_WIDTH_PX,
						mBorderPaint);
			}

			if (mHueShader == null)
			{
				//The hue shader has either not yet been created or the view has been resized.
				mHueShader = new LinearGradient(0, 0, 0, rect.Height(), buildHueColorArray(), null, Shader.TileMode.Clamp);
				mHuePaint.SetShader(mHueShader);
			}

			canvas.DrawRect(rect, mHuePaint);

			float rectHeight = 4 * mDensity / 2;

			Point p = hueToPoint(mHue);

			RectF r = new RectF
			{
				Left = rect.Left - RECTANGLE_TRACKER_OFFSET,
				Right = rect.Right + RECTANGLE_TRACKER_OFFSET,
				Top = p.Y - rectHeight,
				Bottom = p.Y + rectHeight
			};


			canvas.DrawRoundRect(r, 2, 2, mHueAlphaTrackerPaint);

			//Log.d("mColorPicker", "Draw Time Hue: " + (SystemClock.elapsedRealtime() - start) + "ms");

		}


		private Point hueToPoint(float hue)
		{

			RectF rect = mHueRect;
			float height = rect.Height();

			Point p = new Point { Y = (int)(height - (hue * height / 360f) + rect.Top), X = (int)rect.Left };

			return p;
		}

		private Point satValToPoint(float sat, float val)
		{

			RectF rect = mSatValRect;
			float height = rect.Height();
			float width = rect.Width();

			Point p = new Point();

			p.X = (int)(sat * width + rect.Left);
			p.Y = (int)((1f - val) * height + rect.Top);

			return p;
		}

		private Point alphaToPoint(int alpha)
		{

			RectF rect = mAlphaRect;
			float width = rect.Width();

			Point p = new Point();

			p.X = (int)(width - (alpha * width / 0xff) + rect.Left);
			p.Y = (int)rect.Top;

			return p;

		}

		private float[] pointToSatVal(float x, float y)
		{

			RectF rect = mSatValRect;
			float[] result = new float[2];

			float width = rect.Width();
			float height = rect.Height();

			if (x < rect.Left)
			{
				x = 0f;
			}
			else if (x > rect.Right)
			{
				x = width;
			}
			else
			{
				x = x - rect.Left;
			}

			if (y < rect.Top)
			{
				y = 0f;
			}
			else if (y > rect.Bottom)
			{
				y = height;
			}
			else
			{
				y = y - rect.Top;
			}


			result[0] = 1f / width * x;
			result[1] = 1f - (1f / height * y);

			return result;
		}

		private float pointToHue(float y)
		{

			RectF rect = mHueRect;

			float height = rect.Height();

			if (y < rect.Top)
			{
				y = 0f;
			}
			else if (y > rect.Bottom)
			{
				y = height;
			}
			else
			{
				y = y - rect.Top;
			}

			return 360f - (y * 360f / height);
		}

		private int pointToAlpha(int x)
		{

			RectF rect = mAlphaRect;
			int width = (int)rect.Width();

			if (x < rect.Left)
			{
				x = 0;
			}
			else if (x > rect.Right)
			{
				x = width;
			}
			else
			{
				x = x - (int)rect.Left;
			}

			return 0xff - (x * 0xff / width);

		}


		public override bool OnTrackballEvent(MotionEvent e)
		{
			float x = e.GetX();
			float y = e.GetY();

			bool update = false;

			if (e.Action == MotionEventActions.Move)
			{

				switch (mLastTouchedPanel)
				{

					case PANEL_SAT_VAL:

						float sat, val;

						sat = mSat + x / 50f;
						val = mVal - y / 50f;

						if (sat < 0f)
						{
							sat = 0f;
						}
						else if (sat > 1f)
						{
							sat = 1f;
						}

						if (val < 0f)
						{
							val = 0f;
						}
						else if (val > 1f)
						{
							val = 1f;
						}

						mSat = sat;
						mVal = val;

						update = true;

						break;

					case PANEL_HUE:

						float hue = mHue - y * 10f;

						if (hue < 0f)
						{
							hue = 0f;
						}
						else if (hue > 360f)
						{
							hue = 360f;
						}

						mHue = hue;

						update = true;

						break;

					case PANEL_ALPHA:

						if (!mShowAlphaPanel || mAlphaRect == null)
						{
							update = false;
						}
						else
						{

							int alpha = (int)(mAlpha - x * 10);

							if (alpha < 0)
							{
								alpha = 0;
							}
							else if (alpha > 0xff)
							{
								alpha = 0xff;
							}

							mAlpha = alpha;


							update = true;
						}

						break;
				}


			}


			if (update)
			{
				Color color = Color.HSVToColor(mAlpha, new float[] {mHue, mSat, mVal});
				RaiseColorChanged(color.ToArgb());
				
				Invalidate();
				return true;
			}


			return base.OnTrackballEvent(e);
		}

		public override bool OnTouchEvent(MotionEvent eventArgs)
		{
			bool update = false;

			switch (eventArgs.Action)
			{

				case MotionEventActions.Down:
					mStartTouchPoint = new Point((int)eventArgs.GetX(), (int)eventArgs.GetY());
					update = moveTrackersIfNeeded(eventArgs);
					break;

				case MotionEventActions.Move:
					update = moveTrackersIfNeeded(eventArgs);
					break;
				case MotionEventActions.Up:
					mStartTouchPoint = null;
					update = moveTrackersIfNeeded(eventArgs);
					break;
			}

			if (update)
			{
				RaiseColorChanged(Color.HSVToColor(mAlpha, new float[] { mHue, mSat, mVal }).ToArgb());
				this.Invalidate();
				return true;
			}

			return base.OnTouchEvent(eventArgs);
		}

		private bool moveTrackersIfNeeded(MotionEvent eventArgs)
		{

			if (mStartTouchPoint == null)
			{
				return false;
			}

			bool update = false;

			int startX = mStartTouchPoint.X;
			int startY = mStartTouchPoint.Y;


			if (mHueRect.Contains(startX, startY))
			{
				mLastTouchedPanel = PANEL_HUE;

				mHue = pointToHue(eventArgs.GetY());

				update = true;
			}
			else if (mSatValRect.Contains(startX, startY))
			{

				mLastTouchedPanel = PANEL_SAT_VAL;

				float[] result = pointToSatVal(eventArgs.GetX(), eventArgs.GetY());

				mSat = result[0];
				mVal = result[1];

				update = true;
			}
			else if (mAlphaRect != null && mAlphaRect.Contains(startX, startY))
			{

				mLastTouchedPanel = PANEL_ALPHA;

				mAlpha = pointToAlpha((int)eventArgs.GetX());

				update = true;
			}


			return update;
		}


		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int Width = 0;
			int Height = 0;

			MeasureSpecMode widthMode = MeasureSpec.GetMode(widthMeasureSpec);
			MeasureSpecMode heightMode = MeasureSpec.GetMode(heightMeasureSpec);

			int widthAllowed = MeasureSpec.GetSize(widthMeasureSpec);
			int heightAllowed = MeasureSpec.GetSize(heightMeasureSpec);


			//Log.d("color-picker-view", "widthMode: " + modeToString(widthMode) + " heightMode: " + modeToString(heightMode) + " widthAllowed: " + widthAllowed + " heightAllowed: " + heightAllowed);

			if (widthMode == MeasureSpecMode.Exactly || heightMode == MeasureSpecMode.Exactly)
			{
				//A exact value has been set in either direction, we need to stay within this size.

				if (widthMode == MeasureSpecMode.Exactly && heightMode != MeasureSpecMode.Exactly)
				{
					//The with has been specified exactly, we need to adopt the height to fit.
					int h = (int)(widthAllowed - PANEL_SPACING - HUE_PANEL_WIDTH);

					if (mShowAlphaPanel)
					{
						h += (int)(PANEL_SPACING + ALPHA_PANEL_HEIGHT);
					}

					if (h > heightAllowed)
					{
						//We can't fit the view in this container, set the size to whatever was allowed.
						Height = heightAllowed;
					}
					else
					{
						Height = h;
					}

					Width = widthAllowed;

				}
				else if (heightMode == MeasureSpecMode.Exactly && widthMode != MeasureSpecMode.Exactly)
				{
					//The height has been specified exactly, we need to stay within this height and adopt the width.

					int w = (int)(heightAllowed + PANEL_SPACING + HUE_PANEL_WIDTH);

					if (mShowAlphaPanel)
					{
						w -= (int)(PANEL_SPACING - ALPHA_PANEL_HEIGHT);
					}

					if (w > widthAllowed)
					{
						//we can't fit within this container, set the size to whatever was allowed.
						Width = widthAllowed;
					}
					else
					{
						Width = w;
					}

					Height = heightAllowed;

				}
				else
				{
					//If we get here the dev has set the width and height to exact sizes. For example match_parent or 300dp.
					//This will mean that the sat/val panel will not be square but it doesn't matter. It will work anyway.
					//In all other senarios our goal is to make that panel square.

					//We set the sizes to exactly what we were told.
					Width = widthAllowed;
					Height = heightAllowed;
				}

			}
			else
			{
				//If no exact size has been set we try to make our view as big as possible 
				//within the allowed space.

				//Calculate the needed with to layout the view based on the allowed height.
				int widthNeeded = (int)(heightAllowed + PANEL_SPACING + HUE_PANEL_WIDTH);
				//Calculate the needed height to layout the view based on the allowed width.
				int heightNeeded = (int)(widthAllowed - PANEL_SPACING - HUE_PANEL_WIDTH);

				if (mShowAlphaPanel)
				{
					widthNeeded -= (int)(PANEL_SPACING + ALPHA_PANEL_HEIGHT);
					heightNeeded += (int)(PANEL_SPACING + ALPHA_PANEL_HEIGHT);
				}


				if (widthNeeded <= widthAllowed)
				{
					Width = widthNeeded;
					Height = heightAllowed;
				}
				else if (heightNeeded <= heightAllowed)
				{
					Height = heightNeeded;
					Width = widthAllowed;
				}
			}

			//Log.d("mColorPicker", " Size: " + Width + "x" + Height);

			SetMeasuredDimension(Width, Height);
		}

		private String modeToString(MeasureSpecMode mode)
		{
			switch (mode)
			{
				case MeasureSpecMode.AtMost:
					return "AT MOST";
				case MeasureSpecMode.Exactly:
					return "EXACTLY";
				case MeasureSpecMode.Unspecified:
					return "UNSPECIFIED";
			}

			return "ERROR";
		}

		private int getPreferredWidth()
		{
			//Our preferred width and height is 200dp for the square sat / val rectangle.
			int width = (int)(200 * mDensity);

			return (int)(width + HUE_PANEL_WIDTH + PANEL_SPACING);
		}

		private int getPreferredHeight()
		{
			int height = (int)(200 * mDensity);

			if (mShowAlphaPanel)
			{
				height += (int)(PANEL_SPACING + ALPHA_PANEL_HEIGHT);
			}
			return height;
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			mDrawingRect = new RectF();
			mDrawingRect.Left = mDrawingOffset + PaddingLeft;
			mDrawingRect.Right = w - mDrawingOffset - PaddingRight;
			mDrawingRect.Top = mDrawingOffset + PaddingTop;
			mDrawingRect.Bottom = h - mDrawingOffset - PaddingBottom;

			//The need to be recreated because they depend on the size of the view.
			mValShader = null;
			mSatShader = null;
			mHueShader = null;
			;

			setUpSatValRect();
			setUpHueRect();
			setUpAlphaRect();
		}

		private void setUpSatValRect()
		{
			//Calculate the size for the big color rectangle.
			RectF dRect = mDrawingRect;

			float left = dRect.Left + BORDER_WIDTH_PX;
			float top = dRect.Top + BORDER_WIDTH_PX;
			float bottom = dRect.Bottom - BORDER_WIDTH_PX;
			float right = dRect.Right - BORDER_WIDTH_PX - PANEL_SPACING - HUE_PANEL_WIDTH;


			if (mShowAlphaPanel)
			{
				bottom -= (ALPHA_PANEL_HEIGHT + PANEL_SPACING);
			}

			mSatValRect = new RectF(left, top, right, bottom);
		}

		private void setUpHueRect()
		{
			//Calculate the size for the hue slider on the left.
			RectF dRect = mDrawingRect;

			float left = dRect.Right - HUE_PANEL_WIDTH + BORDER_WIDTH_PX;
			float top = dRect.Top + BORDER_WIDTH_PX;
			float bottom = dRect.Bottom - BORDER_WIDTH_PX - (mShowAlphaPanel ? (PANEL_SPACING + ALPHA_PANEL_HEIGHT) : 0);
			float right = dRect.Right - BORDER_WIDTH_PX;

			mHueRect = new RectF(left, top, right, bottom);
		}

		private void setUpAlphaRect()
		{

			if (!mShowAlphaPanel) return;

			RectF dRect = mDrawingRect;

			float left = dRect.Left + BORDER_WIDTH_PX;
			float top = dRect.Bottom - ALPHA_PANEL_HEIGHT + BORDER_WIDTH_PX;
			float bottom = dRect.Bottom - BORDER_WIDTH_PX;
			float right = dRect.Right - BORDER_WIDTH_PX;

			mAlphaRect = new RectF(left, top, right, bottom);


			//mAlphaPattern = new AlphaPatternDrawable((int) (5 * mDensity));
			//mAlphaPattern.setBounds(Math.round(mAlphaRect.Left), Math
			//		.round(mAlphaRect.Top), Math.round(mAlphaRect.Right), Math
			//		.round(mAlphaRect.Bottom));
		}

		/**
		 * Get the current color this view is showing.
		 * @return the current color.
		 */
		public int getColor()
		{
			return Color.HSVToColor(mAlpha, new float[] { mHue, mSat, mVal });
		}

		/**
		 * Set the color the view should show.
		 * @param color The color that should be selected.
		 */
		public void setColor(int color)
		{
			setColor(color, false);
		}

		/**
		 * Set the color this view should show.
		 * @param color The color that should be selected.
		 * @param callback If you want to get a callback to
		 * your OnColorChangedListener.
		 */
		public void setColor(int color, bool callback)
		{
			Color c = new Color(color);
			int alpha = c.A;
			int red = c.R;
			int blue = c.B;
			int green = c.G;

			float[] hsv = new float[3];

			Color.RGBToHSV(red, green, blue, hsv);

			mAlpha = alpha;
			mHue = hsv[0];
			mSat = hsv[1];
			mVal = hsv[2];

			if (callback)
			{
				RaiseColorChanged(Color.HSVToColor(mAlpha, new float[] { mHue, mSat, mVal }).ToArgb());
			}

			Invalidate();
		}

		/**
		 * Get the drawing offset of the color picker view.
		 * The drawing offset is the distance from the side of
		 * a panel to the side of the view minus the padding.
		 * Useful if you want to have your own panel below showing
		 * the currently selected color and want to align it perfectly.
		 * @return The offset in pixels.
		 */
		public float getDrawingOffset()
		{
			return mDrawingOffset;
		}

		/**
		 * Set if the user is allowed to adjust the alpha panel. Default is false.
		 * If it is set to false no alpha will be set.
		 * @param visible
		 */
		public void setAlphaSliderVisible(bool visible)
		{

			if (mShowAlphaPanel != visible)
			{
				mShowAlphaPanel = visible;

				/*
				 * Reset all shader to force a recreation. 
				 * Otherwise they will not look right after 
				 * the size of the view has changed.
				 */
				mValShader = null;
				mSatShader = null;
				mHueShader = null;
				;

				RequestLayout();
			}

		}

		/**
		 * Set the color of the tracker slider on the hue and alpha panel.
		 * @param color
		 */
		public void setSliderTrackerColor(uint color)
		{
			mSliderTrackerColor = color;
			mHueAlphaTrackerPaint.Color = new Color((int)mSliderTrackerColor);
			Invalidate();
		}

		/**
		 * Get color of the tracker slider on the hue and alpha panel.
		 * @return
		 */
		public int getSliderTrackerColor()
		{
			return (int)mSliderTrackerColor;
		}

		/**
		 * Set the color of the border surrounding all panels.
		 * @param color
		 */
		public void setBorderColor(uint color)
		{
			mBorderColor = color;
			Invalidate();
		}

		/**
		 * Get the color of the border surrounding all panels.
		 */
		public int getBorderColor()
		{
			return (int)mBorderColor;
		}


		/**
		 * Set the text that should be shown in the 
		 * alpha slider. Set to null to disable text.
		 * @param text Text that should be shown.
		 */
		public void setAlphaSliderText(String text)
		{
			mAlphaSliderText = text;
			Invalidate();
		}

		/**
		 * Get the current value of the text
		 * that will be shown in the alpha
		 * slider.
		 * @return
		 */
		public String getAlphaSliderText()
		{
			return mAlphaSliderText;
		}

		private class BitmapCache
		{
			public Canvas canvas;
			public Bitmap bitmap;
			public float value;
		}
	}
}
