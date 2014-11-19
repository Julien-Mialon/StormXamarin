namespace Storm.Mvvm
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

		public NavigationArgs(NavigationMode _mode)
		{
			Mode = _mode;
		}
	}
}
