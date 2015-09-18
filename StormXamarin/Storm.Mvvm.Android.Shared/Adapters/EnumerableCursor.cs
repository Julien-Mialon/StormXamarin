using System;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.OS;
using Object = Java.Lang.Object;
using Uri = Android.Net.Uri;

namespace Storm.Mvvm.Adapters
{
    public class EnumerableCursor : Object, ILazyCollectionCursor
    {
	    private readonly IEnumerable _collection;
	    private readonly List<object> _list = new List<object>();
	    private readonly IEnumerator _enumerator;

		public Action UpdateCallback { get; set; }

	    private int _enumeratorPosition;

		public int Count { get { return _list.Count; } }
		public int Position { get; private set; }
		public bool IsAfterLast { get; private set; }
		public bool IsBeforeFirst { get; private set; }
		public bool IsFirst { get; private set; }
		public bool IsLast { get; private set; }

	    public EnumerableCursor(IEnumerable source)
	    {
		    if (source == null)
		    {
			    throw new ArgumentNullException("source", "Source of EnumerableCursor could not be null");
		    }

		    _collection = source;
		    _enumerator = _collection.GetEnumerator();

			Position = -1;
		    _enumeratorPosition = -1;

		    ReadNext();

		    IsBeforeFirst = true;
		    IsAfterLast = false;
		    IsFirst = false;
		    IsLast = false;
	    }

	    private bool ReadNext()
	    {
		    if (!_enumerator.MoveNext())
		    {
			    return false;
		    }
		    _list.Add(_enumerator.Current);
		    _enumeratorPosition++;
			CountChanged();

		    return true;
	    }

		public bool Move(int offset)
	    {
			if (offset < 0)
			{
				int newPosition = Position + offset;
				if (newPosition < 0)
				{
					return false;
				}
				Position = newPosition;
			}
			else
			{
				if (Position + 1 < Count)
				{
					//offset can be changed
					offset -= (Count - 1) - Position;
					Position = Count - 1;
					if (offset < 0)
					{
						offset = 0;
					}
				}

				int i = 0;
				for (; i < offset && ReadNext(); i++)
				{
				}
				if (i < offset)
				{
					return false;
				}
				Position += offset;
			}

			return true;
	    }

	    public bool MoveToFirst()
	    {
		    Position = 0;
		    return true;
	    }

	    public bool MoveToLast()
	    {
			// Can not move to last efficiently so just deny the call
		    return false;
	    }

	    public bool MoveToNext()
	    {
		    return Move(1);
	    }

	    public bool MoveToPosition(int position)
	    {
		    // two case, first position is backward and you already have the item
		    if (position < Count)
		    {
			    Position = position;
			    return true;
		    }
			// other case, it's forward and you need to call MoveOffset
		    return Move(position - Count + 1);
	    }

	    public bool MoveToPrevious()
	    {
		    return Move(-1);
	    }

	    public object GetItem()
	    {
		    return _list[Position];
	    }

	    #region Observers

	    private void CountChanged()
	    {
			/*
		    if (_contentObserver != null)
		    {
			    _contentObserver.DispatchChange(true);
		    }
			 */
		    if (UpdateCallback != null)
		    {
			    UpdateCallback();
		    }
	    }

	    private ContentObserver _contentObserver;
	    public void RegisterContentObserver(ContentObserver observer)
	    {
		    _contentObserver = observer;
	    }

	    public void RegisterDataSetObserver(DataSetObserver observer)
	    {
		    _contentObserver = null;
	    }

	    public void UnregisterContentObserver(ContentObserver observer)
	    {
		    //TODO: check
	    }

	    public void UnregisterDataSetObserver(DataSetObserver observer)
	    {
		    //TODO: check
	    }

	    #endregion
		
		#region Tocheck items

		public Uri NotificationUri { get; private set; }
	    public bool WantsAllOnMoveCalls { get; private set; }
		public int ColumnCount { get; private set; }
		public Bundle Extras { get; private set; }


		public void Deactivate()
		{
			//TODO :check what it supposed to do ?
		}
		public bool Requery()
		{
			//TODO: check what it supposed to do
			return false;
		}

		public Bundle Respond(Bundle extras)
		{
			//TODO : check what it supposed to do
			return null;
		}

		public void SetNotificationUri(ContentResolver cr, Uri uri)
		{
			//TODO : check
			throw new NotImplementedException();
		}


		#endregion

		#region Dont care items

		public bool IsClosed { get; private set; }

		public void Close()
		{
			// ignore, closing collection don't have sens
			IsClosed = true;
		}




		#endregion

		#region Useless accessor

		public void CopyStringToBuffer(int columnIndex, CharArrayBuffer buffer)
	    {
		    throw new NotImplementedException();
	    }

	    public byte[] GetBlob(int columnIndex)
	    {
		    throw new NotImplementedException();
	    }

	    public int GetColumnIndex(string columnName)
	    {
		    return 0;
	    }

	    public int GetColumnIndexOrThrow(string columnName)
	    {
		    return 0;
	    }

	    public string GetColumnName(int columnIndex)
	    {
		    return "_id";
	    }

	    public string[] GetColumnNames()
	    {
		    return new string[]{"_id"};
	    }

	    public double GetDouble(int columnIndex)
	    {
		    return 0.0;
	    }

	    public float GetFloat(int columnIndex)
	    {
		    return 0f;
	    }

	    public int GetInt(int columnIndex)
	    {
		    return Position;
	    }

	    public long GetLong(int columnIndex)
	    {
		    return Position;
	    }

	    public short GetShort(int columnIndex)
	    {
		    return 0;
	    }

	    public string GetString(int columnIndex)
	    {
		    return string.Empty;
	    }

	    public FieldType GetType(int columnIndex)
	    {
		    return FieldType.Integer;
	    }

	    public bool IsNull(int columnIndex)
	    {
		    return false;
	    }

	    #endregion

    }
}
