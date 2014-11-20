namespace Storm.Mvvm.Services
{
	public class NavigationArgs
	{
		public enum NavigationMode
		{
			Back,
			Forward,
			New,
			Refresh,
			Reset,
		}

		public NavigationMode Mode { get; private set; }

		public NavigationArgs(NavigationMode mode)
		{
			Mode = mode;
		}
	}
}
