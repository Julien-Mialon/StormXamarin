using System.Threading.Tasks;

namespace TestApp.Business.Interfaces
{
	public interface IImagePickerService
	{
		Task<string> LaunchImagePickerAsync();
	}
}
