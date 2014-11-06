using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storm.Mvvm;

namespace TestApp.Business
{
	public class ViewModelsLocator
	{
		private static IContainer _container;

		public static void Initialize(IContainer _container)
		{
			_container.RegisterFactory<MainPageViewModel>(container => new MainPageViewModel(container));

			ViewModelsLocator._container = _container;
		}

		public MainPageViewModel MainPageViewModel
		{
			get { return _container.Resolve<MainPageViewModel>(); }
		}
	}
}
