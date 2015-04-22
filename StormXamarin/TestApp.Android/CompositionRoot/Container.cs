using System;
using System.Collections.Generic;
using Android.App;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using TestApp.Android.Service;
using TestApp.Business;
using TestApp.Business.Interfaces;

namespace TestApp.Android.CompositionRoot
{
	public class Container : AndroidContainer
	{
		public static readonly ViewModelsLocator ViewModelsLocator = new ViewModelsLocator();

		protected override void Initialize(Application application, Dictionary<string, Type> views, Dictionary<string, Type> dialogs)
		{
			base.Initialize(application, views, dialogs);

			RegisterInstance<IImagePickerService>(new ImagePickerService());
			RegisterInstance<ILocalizationService>(new LocalizationService(application, typeof(Resource.String)));
		}

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);
		}
	}
}
