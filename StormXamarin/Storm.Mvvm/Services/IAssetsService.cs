using System.IO;

namespace Storm.Mvvm.Services
{
	public interface IAssetsService
	{
		Stream OpenAssets(string path);
	}
}
