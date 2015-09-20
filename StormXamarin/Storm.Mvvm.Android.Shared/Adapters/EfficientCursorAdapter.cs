using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Adapters
{
	public class EfficientCursorAdapter : BaseAdapter<object>, IMvvmAdapter, ISearchableAdapter
	{
		private ITemplateSelector _templateSelector;
		private ILazyCollectionCursor _lazyCursor;

		public object Collection
		{
			get { return _lazyCursor; }
			set
			{
				if (!object.Equals(_lazyCursor, value))
				{
					_lazyCursor = value as ILazyCollectionCursor;
					if (_lazyCursor == null)
					{
						throw new InvalidOperationException("Issue CursorAdapter");
					}
					_lazyCursor.UpdateCallback = NotifyDataChanged;
					NotifyDataChanged();
				}
			}
		}

		public ITemplateSelector TemplateSelector
		{
			get { return _templateSelector; }
			set
			{
				if (!object.Equals(_templateSelector, value))
				{
					_templateSelector = value;
					NotifyDataChanged();
				}
			}
		}

		public override object this[int position]
		{
			get
			{
				if (_lazyCursor.Position != position)
				{
					_lazyCursor.MoveToPosition(position);
				}
				return _lazyCursor.GetItem();
			}
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			return TemplateSelector.GetView(this[position], parent, convertView);
		}

		public override int Count
		{
			get
			{
				return _lazyCursor.Count;
			}
		}

		public int IndexOf(object value)
		{
			//TODO :change !
			return 0;//_collection == null ? -1 : _collection.IndexOf(value);
		}
		
		private void NotifyDataChanged()
		{
			if (TemplateSelector != null)
			{
				NotifyDataSetChanged();
			}
		}
	}
}
