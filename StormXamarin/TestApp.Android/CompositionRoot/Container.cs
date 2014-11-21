using Storm.Mvvm.Inject;
using TestApp.Business;

namespace TestApp.Android.CompositionRoot
{
	public class Container : AndroidContainer
	{
		public static readonly ViewModelsLocator ViewModelsLocator = new ViewModelsLocator();

		protected override void Initialize()
		{
			base.Initialize();
			ViewModelsLocator.Initialize(this);
		}
	}
}
