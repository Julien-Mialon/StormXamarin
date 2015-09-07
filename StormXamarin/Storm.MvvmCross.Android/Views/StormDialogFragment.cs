using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cirrious.MvvmCross.Droid.FullFragging.Fragments;
using Cirrious.MvvmCross.ViewModels;
using Storm.MvvmCross.Bindings;
using Storm.MvvmCross.Bindings.Internal;

namespace Storm.MvvmCross.Android.Views
{
	public class StormDialogFragment : MvxDialogFragment, INotifyPropertyChanged
	{
		public override IMvxViewModel ViewModel
		{
			get { return base.ViewModel; }
			set
			{
				base.ViewModel = value;
				if (value != null)
				{
					OnViewModelSet();
				}
			}
		}

		protected virtual void OnViewModelSet()
		{
			BindingProcessor.ProcessBinding(ViewModel, this, ListBindingPath());
		}

		protected virtual List<BindingObject> ListBindingPath()
		{
			return new List<BindingObject>();
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected virtual bool SetProperty<TValue>(ref TValue storage, TValue value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChanged(propertyName);

			return true;
		}

		#endregion	
	}

	public class StormDialogFragment<TViewModel> : StormDialogFragment, IMvxFragmentView<TViewModel> where TViewModel : class, IMvxViewModel
	{
		public new virtual TViewModel ViewModel
		{
			get { return (TViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}