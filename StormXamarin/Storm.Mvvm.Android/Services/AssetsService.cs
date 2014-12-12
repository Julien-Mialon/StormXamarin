using System.IO;
using Storm.Mvvm.Inject;

namespace Storm.Mvvm.Services
{
	public class AssetsService : IAssetsService
	{
		public Stream OpenAssets(string path)
		{
			return CurrentActivity.Assets.Open(path);
		}
	}
}
