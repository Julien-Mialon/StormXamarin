using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.ViewSelectors;
using Object = Java.Lang.Object;

namespace Storm.Mvvm
{
	public class BindableArrayAdapter<T> : BaseAdapter<T>
	{
		private readonly IEnumerable<T> _dataSource;
		private readonly Activity _context;
		private readonly LayoutInflater _layoutInflater;
		private readonly IViewSelector _viewSelector;

		protected BindableArrayAdapter(IEnumerable<T> dataSource, Activity context)
		{
			_layoutInflater = context.LayoutInflater;
			_context = context;
			_dataSource = dataSource;
		}

		public BindableArrayAdapter(IEnumerable<T> dataSource, Activity context, int viewId)
			: this(dataSource, context)
		{
			_viewSelector = new SimpleViewSelector(_layoutInflater, viewId);
		}

		public BindableArrayAdapter(IEnumerable<T> dataSource, Activity context, IViewSelector viewSelector)
			: this(dataSource, context)
		{
			_viewSelector = viewSelector;
		}

		public override Object GetItem(int position)
		{
			throw new NotImplementedException();
		}

		public override T this[int position]
		{
			get { return _dataSource.ElementAt(position); }
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			return _viewSelector.GetView(this[position], parent, convertView);
		}

		public override int Count
		{
			get { return _dataSource.Count(); }
		}
	}
}
