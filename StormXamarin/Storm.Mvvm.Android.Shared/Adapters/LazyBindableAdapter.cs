using System;
using Android.Content;
using Android.Database;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Adapters
{
    public class LazyBindableAdapter : CursorAdapter
    {
		private ITemplateSelector _templateSelector;
		public ITemplateSelector TemplateSelector
		{
			get { return _templateSelector; }
			set
			{
				if (!object.Equals(_templateSelector, value))
				{
					_templateSelector = value;
					if (value != null)
					{
						NotifyDataSetChanged();
					}
				}
			}
		}

	    #region Constructors

	    public LazyBindableAdapter(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
	    {
	    }

	    public LazyBindableAdapter(Context context, ILazyCollectionCursor c) : base(context, c)
	    {
		    c.UpdateCallback = NotifyDataSetChanged;
	    }

		public LazyBindableAdapter(Context context, ILazyCollectionCursor c, bool autoRequery)
			: base(context, c, autoRequery)
	    {
			c.UpdateCallback = NotifyDataSetChanged;
	    }

		public LazyBindableAdapter(Context context, ILazyCollectionCursor c, CursorAdapterFlags flags)
			: base(context, c, flags)
	    {
			c.UpdateCallback = NotifyDataSetChanged;
	    }

	    #endregion
		
	    public override void BindView(View view, Context context, ICursor cursor)
	    {
			//TODO : To check, can create problem
			// check if NewView is called before any BindView call (and on the same view)
			// in this case, just remove the call below since binding is done when calling GetView on
			// TemplateSelector
			// Is CursorAdapter virtualized ? (reuse old views)
		    
			object item = ((ILazyCollectionCursor)cursor).GetItem();
			TemplateSelector.GetView(item, null, view);

		    Log.Wtf("ADAPTER", "BindView() with item {0}", item);
	    }

	    public override View NewView(Context context, ICursor cursor, ViewGroup parent)
	    {
		    object item = ((ILazyCollectionCursor) cursor).GetItem();
		    Log.Wtf("ADAPTER", "NewView() with item {0}", item);
		    return TemplateSelector.GetView(item, parent, null);
	    }
    }
}
