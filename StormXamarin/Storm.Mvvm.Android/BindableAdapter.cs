using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.ViewSelectors;
using Object = Java.Lang.Object;

namespace Storm.Mvvm
{
	public class BindableAdapter : BaseAdapter<object>
	{
		private readonly IViewSelector _viewSelector;
		private IList _collection;

		public object Collection
		{
			get { return _collection; }
			set
			{
				if (!object.Equals(_collection, value))
				{
					Unregister(_collection);
					_collection = value as IList;
					Register(_collection);
					NotifyDataSetChanged();
				}
			}
		}

		public BindableAdapter(IViewSelector viewSelector)
		{
			_viewSelector = viewSelector;
		}

		public override Object GetItem(int position)
		{
			return null;
		}

		public override object this[int position]
		{
			get { return _collection == null ? null : _collection[position]; }
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
			get
			{
				return _collection == null ? 0 : _collection.Count;
			}
		}

		private void Unregister(object collection)
		{
			if (collection is INotifyCollectionChanged)
			{
				INotifyCollectionChanged observable = collection as INotifyCollectionChanged;
				observable.CollectionChanged -= OnCollectionChanged;
			}
		}

		private void Register(object collection)
		{
			if (collection is INotifyCollectionChanged)
			{
				INotifyCollectionChanged observable = collection as INotifyCollectionChanged;
				observable.CollectionChanged += OnCollectionChanged;
			}
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			NotifyDataSetChanged();
		}
	}
}
