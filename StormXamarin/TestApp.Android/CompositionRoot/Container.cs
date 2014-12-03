using System;
using System.Collections.Generic;
using Android.App;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Services;
using TestApp.Business;

namespace TestApp.Android.CompositionRoot
{
	public class Container : AndroidContainer
	{
		public static readonly ViewModelsLocator ViewModelsLocator = new ViewModelsLocator();

		protected override void Initialize(Application application, Dictionary<string, Type> views)
		{
			base.Initialize(application, views);

			ILocalizationService localizationService = new LocalizationService(application, typeof(Resource.String));
			RegisterInstance(localizationService);
		}

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);

			
		}
	}
}
