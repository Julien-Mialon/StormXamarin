using System;
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

		protected INavigationService NavigationService
		{
			get { return LazyResolver<INavigationService>.Service; }
		}

		protected IDispatcherService DispatcherService
		{
			get { return LazyResolver<IDispatcherService>.Service; }
		}

		protected ILoggerService LoggerService
		{
			get { return LazyResolver<ILoggerService>.Service; }
		}

		protected NavigationParametersContainer NavigationParameters;

		#endregion

		#region Public methods

		public virtual void OnNavigatedFrom(NavigationArgs e)
		{

		}

		public virtual void OnNavigatedTo(NavigationArgs e, string parametersKey)
		{
			// Load parameters only the first time you arrive on the view
			if (e.Mode == NavigationArgs.NavigationMode.New)
			{
				NavigationParameters = NavigationService.GetParameters(parametersKey);

				if (NavigationParameters != null)
				{
					//Process auto navigation parameters property
					foreach (PropertyInfo property in GetType().GetRuntimeProperties().Where(x => x.GetCustomAttribute<NavigationParameterAttribute>(true) != null))
					{
						NavigationParameterAttribute attribute = property.GetCustomAttribute<NavigationParameterAttribute>(true);

						string parameterName = attribute.Name ?? property.Name;
						if (attribute.Mode == NavigationParameterMode.Required && !NavigationParameters.Has(parameterName))
						{
							throw new ArgumentOutOfRangeException(string.Format("Missing required navigation parameter {0}", parameterName));
						}

						if (NavigationParameters.Has(parameterName))
						{
							object keyValue = NavigationParameters.Get<object>(parameterName);

							property.SetValue(this, keyValue);
						}
					}
				}
			}
		}

		#endregion
	}
}
