using Android.App;
using Storm.Mvvm.Inject;
using Storm.Mvvm.Interfaces;

namespace Storm.Mvvm.Services
{
	public abstract class AbstractServiceWithActivity
	{
		protected IActivityService ActivityService
		{
			get { return LazyResolver<IActivityService>.Service; }
		}

		protected Activity CurrentActivity
		{
			get { return ActivityService.CurrentActivity; }
		}
	}
}
