using System;
using Android.Database;

namespace Storm.Mvvm.Adapters
{
    public interface ILazyCollectionCursor
    {
	    event EventHandler CountChanged;
	    event EventHandler CollectionChanged;

		object this[int position] { get; }

		int Count { get; }
    }
}
