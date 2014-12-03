using System.IO;
using Android.App;
using Storm.Mvvm.Inject;

namespace Storm.Mvvm.Services
{
	public class AssetsService : IAssetsService, IActivityUpdatable
	{
		private Activity _activity;

		public Stream OpenAssets(string path)
		{
			return _activity.Assets.Open(path);
		}

		public void UpdateActivity(Activity activity)
		{
			_activity = activity;
		}
	}
}
