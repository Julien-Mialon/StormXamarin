using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Cirrious.MvvmCross.Droid.Views;
using Cirrious.MvvmCross.ViewModels;

namespace Storm.MvvmCross.Android
{
	public class StormActivity : MvxActivity, INotifyPropertyChanged
	{
		protected override void OnViewModelSet()
		{
			base.OnViewModelSet();
		}

		protected virtual List<BindingObject> ListBindingPath()
		{
			
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

		protected virtual bool SetProperty<TValue>(ref TValue _storage, TValue value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(_storage, value))
			{
				return false;
			}

			_storage = value;
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChanged(propertyName);

			return true;
		}

		#endregion

	}

	public class StormActivity<TViewModel> : MvxActivity<TViewModel>, INotifyPropertyChanged where TViewModel : class, IMvxViewModel
	{
		protected override void OnViewModelSet()
		{
			base.OnViewModelSet();
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

		protected virtual bool SetProperty<TValue>(ref TValue _storage, TValue value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(_storage, value))
			{
				return false;
			}

			_storage = value;
			// ReSharper disable once ExplicitCallerInfoArgument
			OnPropertyChanged(propertyName);

			return true;
		}

		#endregion

	}
}