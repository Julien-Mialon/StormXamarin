using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Storm.Mvvm.Navigation;
using Storm.Mvvm.Patterns;
using Storm.Mvvm.Services;

namespace Storm.Mvvm
{
	public interface IViewModelLifecycle
	{
		Task OnResume();
		Task OnPause();
	}

	public class ViewModelBase : NotifierBase, IViewModelLifecycle
	{
		private Dictionary<string, object> _navigationParameters;

		public INavigationService NavigationService => LazySingletonInitializer<INavigationService>.Value;

		public virtual void Initialize(Dictionary<string, object> navigationParameters)
		{
			_navigationParameters = navigationParameters;

			if (_navigationParameters != null)
			{
				//Process auto navigation parameters property
				foreach (PropertyInfo property in GetType().GetRuntimeProperties().Where(x => x.GetCustomAttribute<NavigationParameterAttribute>(true) != null))
				{
					NavigationParameterAttribute attribute = property.GetCustomAttribute<NavigationParameterAttribute>(true);

					string parameterName = attribute.Name ?? property.Name;
					object keyValue = GetNavigationParameter<object>(parameterName);

					property.SetValue(this, keyValue);
				}
			}
		}

		public virtual Task OnPause() => Task.CompletedTask;

		public virtual Task OnResume() => Task.CompletedTask;

		protected T GetNavigationParameter<T>(string key)
		{
			if (_navigationParameters.ContainsKey(key))
			{
				return (T)_navigationParameters[key];
			}
			return default(T);
		}
	}
}
