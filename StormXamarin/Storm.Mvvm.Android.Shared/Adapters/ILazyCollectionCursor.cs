using System;
using Android.Database;

namespace Storm.Mvvm.Adapters
{
    public interface ILazyCollectionCursor : ICursor
    {
		Action UpdateCallback { get; set; }

	    object GetItem();
    }
}
