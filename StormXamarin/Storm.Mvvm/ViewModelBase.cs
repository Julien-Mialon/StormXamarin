using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public abstract class ViewModelBase : NotifierBase
	{
		#region Fields

		protected IContainer Container = null;
		protected INavigationService NavigationService = null;
		protected IDispatcherService DispatcherService = null;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		protected ViewModelBase(IContainer container)
		{
			Container = container;
			NavigationService = Container.Resolve<INavigationService>();
			DispatcherService = Container.Resolve<IDispatcherService>();
		}

		#endregion

		#region Public methods

		public virtual void OnNavigatedFrom(NavigationArgs e)
		{

		}

		public virtual void OnNavigatedTo(NavigationArgs e)
		{

		}

		#endregion
	}
}
