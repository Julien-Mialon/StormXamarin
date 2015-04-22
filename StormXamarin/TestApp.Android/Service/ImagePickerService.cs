using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;
using Storm.Mvvm.Services;
using TestApp.Business.Interfaces;
using Uri = Android.Net.Uri;

namespace TestApp.Android.Service
{
	class ImagePickerService : IImagePickerService
	{
		public void LaunchImagePicker()
		{
			IActivityService activityService = LazyResolver<IActivityService>.Service;

			var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
			activityService.StartActivityForResult(intent, (result, data) =>
			{
				if (result == Result.Ok)
				{
					Uri selectedImage = data.Data;
					LazyResolver<ILoggerService>.Service.Log("Photo picked: " + selectedImage.Path,MessageSeverity.Critical);
				}
				else
				{
					LazyResolver<ILoggerService>.Service.Log("Pick photo canceled", MessageSeverity.Critical);
				}
			});
		}
	}
}