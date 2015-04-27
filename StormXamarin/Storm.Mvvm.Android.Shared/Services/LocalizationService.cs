using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Content;

namespace Storm.Mvvm.Services
{
	public class LocalizationService : ILocalizationService
	{
		private readonly Dictionary<string, string> _strings; 

		public LocalizationService(Context context, Type stringType)
		{
			_strings = stringType.GetRuntimeFields()
								.Where(field => field.IsLiteral)
								.ToDictionary(
									field => field.Name, 
									field => context.GetString((int)field.GetRawConstantValue())
								);
		}

		public string GetString(string uid)
		{
			return (string.IsNullOrEmpty(uid) || !_strings.ContainsKey(uid)) ? "" : _strings[uid];
		}

		public string GetString(string uid, string property)
		{
			string key = string.Format("{0}__{1}", uid, property);
			return (string.IsNullOrEmpty(uid) || !_strings.ContainsKey(key)) ? "" : _strings[key];
		}
	}
}
