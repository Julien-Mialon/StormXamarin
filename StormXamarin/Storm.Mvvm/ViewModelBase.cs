using System.Linq;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Services;
using System.Reflection;

namespace Storm.Mvvm
{
	public abstract class ViewModelBase : NotifierBase
	{
		#region Fields

		protected IContainer Container;
		protected INavigationService NavigationService;
		protected IDispatcherService DispatcherService;
		protected ILoggerService LoggerService;

		protected NavigationParametersContainer NavigationParameters;

		#endregion

		#region Properties

		#endregion

		#region Constructors

		protected ViewModelBase(IContainer container)
		{
			Container = container;
			NavigationService = Container.Resolve<INavigationService>();
			DispatcherService = Container.Resolve<IDispatcherService>();
			LoggerService = Container.Resolve<ILoggerService>();
		}

		#endregion

		#region Public methods

		public virtual void OnNavigatedFrom(NavigationArgs e)
		{

		}

		public virtual void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			NavigationParameters = NavigationService.GetParameters(parametersKey);

			if (NavigationParameters != null)
			{
				//Process auto navigation parameters property
				foreach (PropertyInfo property in GetType().GetRuntimeProperties().Where(x => x.GetCustomAttribute<NavigationParameterAttribute>(true) != null))
				{
					NavigationParameterAttribute attribute = property.GetCustomAttribute<NavigationParameterAttribute>(true);

					string parameterName = attribute.Name ?? property.Name;
					object keyValue = NavigationParameters.Get<object>(parameterName);

					property.SetValue(this, keyValue);
				}
			}
		}

		#endregion
	}
}
