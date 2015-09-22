using Cirrious.MvvmCross.ViewModels;

namespace Test.MvvmCross.Business
{
	public class App : MvxApplication
	{
		public override void Initialize()
		{
			base.Initialize();

			RegisterAppStart<MainViewModel>();
		}
	}
}
