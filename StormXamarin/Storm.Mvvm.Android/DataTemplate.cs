using System;
using Android.Views;
using Storm.Mvvm.TemplateSelectors;

namespace Storm.Mvvm
{
	public class DataTemplate
	{
		public int ViewId { get; set; }

		public LayoutInflater LayoutInflater { get; set; }

		public Type ViewHolderType { get; set; }

		public View Inflate(ViewGroup parent)
		{
			return LayoutInflater.Inflate(ViewId, parent, false);
		}

		public void AttachToViewModel(View view, object model)
		{
			BaseViewHolder viewHolder = Activator.CreateInstance(ViewHolderType) as BaseViewHolder;

			if (viewHolder == null)
			{
				throw new Exception("Can not find ViewHolder for template with id = " + ViewId + " and type = " + ViewHolderType);
			}

			viewHolder.LayoutInflater = LayoutInflater;
			viewHolder.View = view;

			viewHolder.SetViewModel(model);
		}
	}
}