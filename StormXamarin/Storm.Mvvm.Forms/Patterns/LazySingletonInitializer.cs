using Xamarin.Forms;

namespace Storm.Mvvm.Patterns
{
	public class LazySingletonInitializer<T> where T : class
	{
		public static T Value = DependencyService.Get<T>();
	}
}
