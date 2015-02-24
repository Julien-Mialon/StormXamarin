namespace Storm.Mvvm.Inject
{
	public interface IInjectionFactory
	{
		bool IsSingleFactory { get; }
	}

	public interface IInjectionFactory<out T> : IInjectionFactory
	{
		T Create(IContainer container);
	}
}
