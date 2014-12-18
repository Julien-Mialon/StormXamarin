﻿using Android.Views;

namespace Storm.Mvvm.ViewSelectors
{
	public class SimpleViewSelector : ViewSelectorBase
	{
		public int DefaultView { get; set; }

		public SimpleViewSelector(LayoutInflater layoutInflater) : base(layoutInflater)
		{

		}

		public override int GetViewId(object model)
		{
			return DefaultView;
		}
	}
}
