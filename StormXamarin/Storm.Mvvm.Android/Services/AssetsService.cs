using System.IO;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Services
{
	public class AssetsService : AbstractServiceWithActivity, IAssetsService
	{
		public AssetsService(IActivityService activityService) : base(activityService)
		{
		}

		public Stream OpenAssets(string path)
		{
			return CurrentActivity.Assets.Open(path);
		}
	}
}
