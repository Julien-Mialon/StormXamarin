using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace Storm.Mvvm.Services
{
	public class AssetsService : IAssetsService
	{
		public Stream OpenAssets(string path)
		{
			StorageFolder rootFolder = Package.Current.InstalledLocation;

			Task<Stream> taskResult = rootFolder.OpenStreamForReadAsync(path);
			taskResult.RunSynchronously();

			return taskResult.Result;
		}
	}
}
