using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Storm.Mvvm
{
	public class NotifierBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;
			OnPropertyChanged(propertyName);

			return true;
		}
	}
}
