using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Storm.Mvvm.Navigation;
using Xamarin.Forms;

namespace Storm.Mvvm
{
	public class ViewModelBase : NotifierBase
	{
		private Dictionary<string, object> _navigationParameters;

		public INavigation NavigationService { get; set; }

		public virtual void Initialize(Dictionary<string, object> navigationParameters)
		{
			_navigationParameters = navigationParameters;

			if (_navigationParameters != null)
			{
				//Process auto navigation parameters property
				foreach (PropertyInfo property in this.GetType().GetRuntimeProperties().Where(x => x.GetCustomAttribute<NavigationParameterAttribute>(true) != null))
				{
					NavigationParameterAttribute attribute = property.GetCustomAttribute<NavigationParameterAttribute>(true);

					string parameterName = attribute.Name ?? property.Name;
					object keyValue = GetNavigationParameter<object>(parameterName);

					property.SetValue(this, keyValue);
				}
			}
		}

		protected T GetNavigationParameter<T>(string key)
		{
			if (_navigationParameters.ContainsKey (key)) 
			{
				return (T)_navigationParameters[key];
			}
			return default(T);
		}
	}
}
