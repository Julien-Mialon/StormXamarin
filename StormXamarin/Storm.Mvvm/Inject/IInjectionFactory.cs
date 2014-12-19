namespace Storm.Mvvm.Inject
{
	public interface IInjectionFactory
	{

	}

	public interface IInjectionFactory<out T> : IInjectionFactory
	{
		T Create(IContainer container);
	}
}
