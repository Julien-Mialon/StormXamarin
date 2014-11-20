using System;
using System.Collections.Generic;
using System.Reflection;

namespace Storm.Mvvm.Services
{
	public class LocalizationService : ILocalizationService
	{
		private readonly Dictionary<string, string> _props = new Dictionary<string, string>();

		public LocalizationService(object resource)
		{
			Type type = resource.GetType();
			IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties();

			foreach(PropertyInfo property in properties)
			{
				_props.Add(property.Name, property.GetValue(resource) as string);
			}
		}

		public string GetString(string uid)
		{
			if(string.IsNullOrEmpty(uid) || !_props.ContainsKey(uid))
			{
				return "";
			}
			return _props[uid];
		}

		public string GetString(string uid, string property)
		{
			string key = string.Format("{0}_{1}", uid, property);
			if (string.IsNullOrEmpty(uid) || !_props.ContainsKey(key))
			{
				return "";
			}
			return _props[key];
		}
	}
}
