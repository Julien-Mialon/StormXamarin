using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Storm.Mvvm
{
	public class NotifierBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged([CallerMemberName] string _propertyName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if(handler != null)
			{
				handler(this, new PropertyChangedEventArgs(_propertyName));
			}
		}

		public bool SetProperty<T>(ref T _storage, T _value, [CallerMemberName] string _propertyName = "")
		{
			if(Equals(_storage, _value))
			{
				return false;
			}

			_storage = _value;
			RaisePropertyChanged(_propertyName);

			return true;
		}
	}
}
