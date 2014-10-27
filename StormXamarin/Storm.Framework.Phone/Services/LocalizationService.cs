using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Storm.Framework.Services
{
	public class LocalizationService : ILocalizationService
	{
		private Dictionary<string, string> _props = new Dictionary<string, string>();

		public LocalizationService(object _resource)
		{
			Type type = _resource.GetType();
			IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties();

			foreach(PropertyInfo property in properties)
			{
				_props.Add(property.Name, property.GetValue(_resource) as string);
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
