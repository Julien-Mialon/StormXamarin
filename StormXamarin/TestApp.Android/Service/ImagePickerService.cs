using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;
using TestApp.Business.Interfaces;
using Uri = Android.Net.Uri;

namespace TestApp.Android.Service
{
	class ImagePickerService : IImagePickerService
	{
		public Task<string> LaunchImagePickerAsync()
		{
			return AsyncHelper.CreateAsyncFromCallback<string>(resultCallback =>
			{
				IActivityService activityService = LazyResolver<IActivityService>.Service;
				var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
				activityService.StartActivityForResult(intent, (result, data) =>
				{
					string res = null;
					if (result == Result.Ok)
					{
						Uri selectedImage = data.Data;
						res = selectedImage.Path;
						LazyResolver<ILoggerService>.Service.Log("Photo picked: " + selectedImage.Path, MessageSeverity.Critical);
					}
					else
					{
						LazyResolver<ILoggerService>.Service.Log("Pick photo canceled", MessageSeverity.Critical);
					}
					resultCallback(res);
				});
			});
		}
	}
}