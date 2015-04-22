namespace Storm.Mvvm.Inject
{
	public static class LazyResolver<T> where T : class
	{
		private static T _service;

		public static T Service
		{
			get { return _service ?? (_service = DependencyService.Container.Resolve<T>()); }
		}
	}
}
