using System.Collections;
using System.Collections.Specialized;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm
{
	public class BindableAdapter : BaseAdapter<object>, ISearchableAdapter
	{
		private readonly IViewSelector _viewSelector;
		private IList _collection;

		public object Collection
		{
			get { return _collection; }
			set
			{
				if (!Equals(_collection, value))
				{
					Unregister(_collection);
					_collection = value as IList;
					Register(_collection);
					NotifyDataChanged();
				}
			}
		}

		public BindableAdapter(IViewSelector viewSelector)
		{
			_viewSelector = viewSelector;
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

		public int IndexOf(object value)
		{
			return _collection == null ? -1 : _collection.IndexOf(value);
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
			NotifyDataChanged();
		}

		private void NotifyDataChanged()
		{
			NotifyDataSetChanged();
		}
	}
}
