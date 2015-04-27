using System.IO;

namespace Storm.Mvvm.Services
{
	public class AssetsService : AbstractServiceWithActivity, IAssetsService
	{
		public Stream OpenAssets(string path)
		{
			return CurrentActivity.Assets.Open(path);
		}
	}
}
