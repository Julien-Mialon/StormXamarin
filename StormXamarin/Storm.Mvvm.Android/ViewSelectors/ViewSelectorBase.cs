﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Views;
using Storm.Mvvm.Bindings;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.ViewSelectors
{
	public abstract class ViewSelectorBase : IViewSelector
	{
		private static readonly MethodInfo _staticMethod;

		protected LayoutInflater LayoutInflater { get; private set; }

		public Dictionary<int, List<BindingObject>> BindingDictionary { get; private set; }

		static ViewSelectorBase()
		{
			Assembly assembly = ApplicationBase.MainAssembly;
			Type objectType = assembly.GetType("Storm.Generated.AutoGenerated_ViewHolderFactory");
			if (objectType == null)
			{
				throw new Exception("Can not find ViewHolderFactory, has your project been preprocessed ?");
			}
			MethodInfo method = objectType.GetMethod("Get", BindingFlags.Public | BindingFlags.Static);
			if (method == null)
			{
				throw new Exception("Can not find Get method in ViewHolderFactory, has your project been preprocessed ?");
			}
			_staticMethod = method;
		}

		protected ViewSelectorBase(LayoutInflater layoutInflater)
		{
			LayoutInflater = layoutInflater;
			BindingDictionary = new Dictionary<int, List<BindingObject>>();
		}

		public View GetView(object model, ViewGroup parent, View oldView)
		{
			int newViewId = GetViewId(model);
			int oldViewId = (oldView != null) ? (int)oldView.Tag : -1;

			View resultView;

			if (oldViewId == newViewId)
			{
				resultView = oldView;
			}
			else
			{
				resultView = LayoutInflater.Inflate(newViewId, parent, false);
				resultView.Tag = newViewId;
			}

			AssociateViewWithModel(newViewId, resultView, model);

			return resultView;
		}

		public virtual void AssociateViewWithModel(int viewId, View view, object model)
		{
			BaseViewHolder viewHolder = _staticMethod.Invoke(null, new object[] {this.LayoutInflater, view, viewId}) as BaseViewHolder;
			if (viewHolder == null)
			{
				throw new Exception("Can not find viewHolder for template with id = " + viewId);
			}

			viewHolder.SetViewModel(model);
		}

		public abstract int GetViewId(object model);
	}
}
