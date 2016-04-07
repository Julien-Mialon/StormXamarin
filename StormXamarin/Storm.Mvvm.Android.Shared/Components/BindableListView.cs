using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Storm.Mvvm.Interfaces;
using System;

namespace Storm.Mvvm.Components
{
    public class BindableListView : ListView
    {
        public event EventHandler CurrentItemChanged;

	    private object _currentItem;

        public object CurrentItem
        {
            get { return SelectedItem ?? _currentItem; }
            set
            {
                int position = -1;
                var adapter = Adapter as ISearchableAdapter;

                if (adapter != null)
                {
                    position = adapter.IndexOf(value);
                }

                if (position >= 0)
                {
                    if (SelectedItemPosition != position)
                    {
                        SetSelection(position);
                    }
                }
	            _currentItem = value;
            }
        }

        protected BindableListView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Initialize();
        }

        public BindableListView(Context context) : base(context)
        {
            Initialize();
        }

        public BindableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public BindableListView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        private void Initialize()
        {
			ItemClick += (sender, args) =>
	        {
		        var adapter = Adapter as BaseAdapter;

                if (adapter != null)
                {
	                _currentItem = adapter.GetItem(args.Position);
					OnCurrentItemChanged();
                }
	        };
        }

        protected void OnCurrentItemChanged()
        {
            EventHandler handler = CurrentItemChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}
