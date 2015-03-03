using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App;
using Android.OS;
using Android.Views;
using Storm.Mvvm.Bindings;

namespace Storm.Mvvm
{
	public abstract class FragmentBase : Fragment, INotifyPropertyChanged
	{
		protected ViewModelBase ViewModel { get; private set; }

		protected View RootView { get; private set; }

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			if (RootView != null)
			{
				ViewGroup parent = (ViewGroup)RootView.Parent;
				parent.RemoveView(RootView);
			}
			else
			{
				RootView = CreateView(inflater, container);
			}

			SetViewModel(CreateViewModel());

			return RootView;
		}

		protected abstract View CreateView(LayoutInflater inflater, ViewGroup container);

		protected abstract ViewModelBase CreateViewModel();

		protected void SetViewModel(ViewModelBase viewModel)
		{
			ViewModel = viewModel;
			BindingProcessor.ProcessBinding(ViewModel, this, GetBindingPaths());
		}

		protected virtual List<BindingObject> GetBindingPaths()
		{
			return new List<BindingObject>();
		}

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
		{
			if (Equals(storage, value))
			{
				return false;
			}

			storage = value;
			// ReSharper disable once ExplicitCallerInfoArgument : need it here
			RaisePropertyChanged(propertyName);

			return true;
		}

		#endregion
	}
}
