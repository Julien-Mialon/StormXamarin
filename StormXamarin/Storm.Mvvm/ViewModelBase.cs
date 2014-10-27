using Storm.Framework.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm
{
	public abstract class ViewModelBase : INotifyPropertyChanged
	{
		#region Events

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Fields

		protected IContainer Container = null;
		protected INavigationService NavigationService = null;
		protected IDispatcherService DispatcherService = null;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		public ViewModelBase(IContainer _container) : base()
		{
			this.Container = _container;
			this.NavigationService = this.Container.Resolve<INavigationService>();
			this.DispatcherService = this.Container.Resolve<IDispatcherService>();
		}

		#endregion

		#region Public methods

		public virtual void OnNavigatedFrom(NavigationArgs _e)
		{

		}

		public virtual void OnNavigatedTo(NavigationArgs _e)
		{

		}

		#endregion

		#region Protected methods

		protected void OnPropertyChanged([CallerMemberName] string _propertyName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if(handler != null)
			{
				handler(this, new PropertyChangedEventArgs(_propertyName));
			}
		}

		protected bool SetProperty<T>(ref T _storage, T _value, [CallerMemberName] string _propertyName = "")
		{
			if(object.Equals(_storage, _value))
			{
				return false;
			}

			_storage = _value;
			OnPropertyChanged(_propertyName);
			return true;
		}

		#endregion
	}
}
