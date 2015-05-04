namespace Storm.Mvvm.Services
{
	public class NavigationArgs
	{
		public enum NavigationMode
		{
			Back,
			Forward,
			New
		}

		public NavigationMode Mode { get; private set; }

		public NavigationArgs(NavigationMode mode)
		{
			Mode = mode;
		}
	}
}
