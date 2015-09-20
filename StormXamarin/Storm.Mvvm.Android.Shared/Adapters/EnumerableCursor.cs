using System;
using System.Collections;
using System.Collections.Generic;
using Storm.Mvvm.Events;

namespace Storm.Mvvm.Adapters
{
	//TODO : add support for observable collection ??
    public class EnumerableCursor : ILazyCollectionCursor
    {
	    private readonly List<object> _list = new List<object>();
	    private IEnumerator _enumerator;
	    private IEnumerable _enumerable;
	    private int _position;

	    public event EventHandler CountChanged;
	    public event EventHandler CollectionChanged;

	    public IEnumerable Collection
	    {
			get { return _enumerable; }
		    set
		    {
			    if (!Equals(_enumerable, value))
			    {
				    _enumerable = value;
					Reset();
			    }
		    }
	    }
	    public int Count { get { return _list.Count; } }

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
		}


	    public EnumerableCursor(IEnumerable source)
	    {
		    Collection = source;
	    }

	    private void Reset()
	    {
			_list.Clear();
			_position = 0;
		    if (Collection == null)
		    {
			    _enumerator = null;
			    return;
		    }

		    _enumerator = Collection.GetEnumerator();
		    ReadNext(2);
	    }

	    private bool ReadNext(int count)
	    {
		    bool hasRead = false;
		    for (int i = 0; i < count && _enumerator.MoveNext(); ++i)
		    {
			    hasRead = true;
				_list.Add(_enumerator.Current);
		    }

		    if (!hasRead)
		    {
			    return false;
		    }
		    OnCountChanged();
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

	    protected void OnCountChanged()
	    {
		    this.RaiseEvent(CountChanged);
	    }

	    protected void OnCollectionChanged()
	    {
		    this.RaiseEvent(CollectionChanged);
	    }
    }
}
