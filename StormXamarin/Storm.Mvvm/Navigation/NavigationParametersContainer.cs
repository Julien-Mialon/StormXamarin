using System;
using System.Collections.Generic;

namespace Storm.Mvvm.Navigation
{
	public class NavigationParametersContainer
	{
		public string Key { get; private set; }

		public Dictionary<string, object> Parameters { get; private set; } 

		public NavigationParametersContainer(string view, Dictionary<string, object> parameters)
		{
			Parameters = parameters;
			Key = string.Format("{0}_{1:yyyyMMdd_HHmmss_fffffff}", view, DateTime.Now);
		}

		public bool Has(string key)
		{
			return Parameters.ContainsKey(key);
		}

		public T Get<T>(string key)
		{
			if (Parameters.ContainsKey(key))
			{
				return (T)Parameters[key];
			}
			throw new IndexOutOfRangeException(string.Format("Key {0} does not exists in parameters list", key));
		}

		public T GetOrDefault<T>(string key, T defaultValue = default(T))
		{
			if (Parameters.ContainsKey(key))
			{
				return (T) Parameters[key];
			}
			return defaultValue;
		}

	}
}
