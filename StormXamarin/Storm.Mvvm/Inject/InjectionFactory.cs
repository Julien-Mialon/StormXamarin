using System;

namespace Storm.Mvvm.Inject
{
	public class InjectionFactory<T> : IInjectionFactory, IInjectionFactory<T>
	{
		private Func<IContainer, T> _factory; 

		public InjectionFactory(Func<IContainer, T> factory)
		{
			_factory = factory;
		}

		public T Create(IContainer container)
		{
			return _factory(container);
		}
	}
}
