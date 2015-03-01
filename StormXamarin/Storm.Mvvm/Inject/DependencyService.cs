namespace Storm.Mvvm.Inject
{
	public static class DependencyService
	{
		public static IContainer Container { get; private set; }

		public static void Attach(IContainer container)
		{
			Container = container;
		}

		public static void Detach(IContainer container)
		{
			if (Equals(container, Container))
			{
				Container = null;
			}
		}
	}
}
