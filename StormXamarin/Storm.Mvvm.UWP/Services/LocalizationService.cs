using Windows.ApplicationModel.Resources;

namespace Storm.Mvvm.Services
{
	public class LocalizationService : ILocalizationService
	{
		private readonly ResourceLoader _resourceLoader;


		public LocalizationService()
		{
			_resourceLoader = ResourceLoader.GetForCurrentView();
		}

		public string GetString(string uid)
		{
			if (string.IsNullOrEmpty(uid))
			{
				return "";
			}
			return _resourceLoader.GetString(uid);
		}

		public string GetString(string uid, string property)
		{
			if (string.IsNullOrEmpty(uid))
			{
				return "";
			}
			string key = string.Format("{0}/{1}", uid, property);

			return _resourceLoader.GetString(key);
		}
	}
}
