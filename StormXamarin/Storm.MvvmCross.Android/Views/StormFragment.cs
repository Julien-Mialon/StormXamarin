using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cirrious.MvvmCross.Droid.FullFragging.Fragments;
using Cirrious.MvvmCross.ViewModels;
using Storm.MvvmCross.Bindings;
using Storm.MvvmCross.Bindings.Internal;

namespace Storm.MvvmCross.Android.Views
{
	public class StormFragment : MvxFragment, INotifyPropertyChanged
	{
		public override void OnViewModelSet()
		{
			base.OnViewModelSet();
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

	public class StormFragment<TViewModel> : MvxFragment<TViewModel>, INotifyPropertyChanged
												where TViewModel : class, IMvxViewModel
	{
		public override void OnViewModelSet()
		{
			base.OnViewModelSet();
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
}