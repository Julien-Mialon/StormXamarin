using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Storm.MvvmCross.Android.Adapters
{
	public class EnumerableCursor : ILazyCollectionCursor
	{
		private readonly List<object> _list = new List<object>();
		private IEnumerator _enumerator;
		private IEnumerable _enumerable;
		private int _position;

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public int Count { get { return _list.Count; } }
		public object SyncRoot { get; private set; }
		public bool IsSynchronized { get; private set; }
		public bool IsReadOnly { get; private set; }
		public bool IsFixedSize { get; private set; }

		public IEnumerable Collection
		{
			get { return _enumerable; }
			set
			{
				if (!Equals(_enumerable, value))
				{
					Unregister(_enumerable);
					_enumerable = value;
					Register(_enumerable);
					Reset();
				}
			}
		}

		public object this[int position]
		{
			get
			{
				if (_position != position)
				{
					if (!MoveToPosition(position))
					{
						throw new IndexOutOfRangeException(string.Format("EnumerableCursor out of range, require index {0} in collection which only count {1} items", position, Count));
					}
				}
				return _list[position];
			}
			set { throw new NotSupportedException("SetItem not supported in EnumerableCursor"); }
		}

		public EnumerableCursor(IEnumerable source)
		{
			SyncRoot = new object();
			IsSynchronized = false;
			Collection = source;
		}

		private void Reset()
		{
			IsReadOnly = true;
			IsFixedSize = false;
			_list.Clear();
			_position = 0;
			if (Collection == null)
			{
				_enumerator = null;
				return;
			}

			_enumerator = Collection.GetEnumerator();
			OnCountChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			ReadNext(2);
		}

		private bool ReadNext(int count)
		{
			bool hasRead = false;
			List<object> newItems = new List<object>();
			int newItemIndex = _list.Count;
			for (int i = 0; i < count && _enumerator.MoveNext(); ++i)
			{
				hasRead = true;
				_list.Add(_enumerator.Current);
				newItems.Add(_enumerator.Current);
			}

			if (!hasRead)
			{
				return false;
			}
			OnCountChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newItems, newItemIndex));
			return true;
		}

		public bool MoveToPosition(int position)
		{
			while (position >= Count - 1 && ReadNext(_list.Count))
			{

			}
			if (position >= Count)
			{
				return false;
			}
			_position = position;
			return true;
		}

		public int IndexOf(object value)
		{
			int index;
			while ((index = _list.IndexOf(value)) < 0 && ReadNext(_list.Count))
			{
			}
			return index;
		}

		protected void OnCountChanged(NotifyCollectionChangedEventArgs args)
		{
			NotifyCollectionChangedEventHandler handler = CollectionChanged;
			if (handler != null)
			{
				handler.Invoke(this, args);
			}
		}

		private void Register(IEnumerable value)
		{
			INotifyCollectionChanged collection = value as INotifyCollectionChanged;
			if (collection != null)
			{
				collection.CollectionChanged += OnObservableCollectionChanged;
			}
		}

		private void Unregister(IEnumerable value)
		{
			INotifyCollectionChanged collection = value as INotifyCollectionChanged;
			if (collection != null)
			{
				collection.CollectionChanged -= OnObservableCollectionChanged;
			}
		}

		private void OnObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			Reset();
		}

		#region Not supported Ops

		public IEnumerator GetEnumerator()
		{
			throw new NotSupportedException();
		}

		public void CopyTo(Array array, int index)
		{
			throw new NotSupportedException();
		}

		public int Add(object value)
		{
			throw new NotSupportedException();
		}

		public bool Contains(object value)
		{
			throw new NotSupportedException();
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public void Insert(int index, object value)
		{
			throw new NotSupportedException();
		}

		public void Remove(object value)
		{
			throw new NotSupportedException();
		}

		public void RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		#endregion

	}
}
