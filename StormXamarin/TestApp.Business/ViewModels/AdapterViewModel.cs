using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Storm.Mvvm;
using Storm.Mvvm.Commands;
using Storm.Mvvm.Inject;

namespace TestApp.Business.ViewModels
{
	public class ListItemModel : NotifierBase
	{
		private string _text;
		private int _id;

		public string Text
		{
			get { return _text; }
			set { SetProperty(ref _text, value); }
		}

		public int Id
		{
			get { return _id; }
			set { SetProperty(ref _id, value); }
		}

		public ListItemModel()
		{
			
		}

		public ListItemModel(int id, string text)
		{
			_id = id;
			_text = text;
		}
	}

	public class AdapterViewModel : ViewModelBase
	{
		private ObservableCollection<ListItemModel> _myCollection = new ObservableCollection<ListItemModel>();
		private ListItemModel _selectedItem = null;
		private string _inputText = null;

		public string InputText
		{
			get { return _inputText; }
			set { SetProperty(ref _inputText, value); }
		}

		public ListItemModel SelectedItem
		{
			get { return _selectedItem; }
			set { SetProperty(ref _selectedItem, value); }
		}

		public ObservableCollection<ListItemModel> MyCollection
		{
			get { return _myCollection; }
			set { SetProperty(ref _myCollection, value); }
		}

		public ICommand AddCommand { get; private set; }

		public AdapterViewModel(IContainer container) : base(container)
		{
			//TODO : to use when CommandParameter works again
			//AddCommand = new DelegateCommand<string>(AddAction);

			AddCommand = new DelegateCommand(AddAction);

			int n = 1;
			foreach (string s in new string[] {"Alpha", "Beta", "Omega"})
			{
				MyCollection.Add(new ListItemModel(n++, s));
			}
			SelectedItem = MyCollection[2];
		}

		private void AddAction(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return;
			}
			MyCollection.Add(new ListItemModel(42, input));
			InputText = "";
		}

		private void AddAction()
		{
			AddAction(InputText);
		}
	}
}
