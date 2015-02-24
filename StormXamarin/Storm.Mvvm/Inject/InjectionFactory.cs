using System;

namespace Storm.Mvvm.Inject
{
	public class InjectionFactory<T> : IInjectionFactory, IInjectionFactory<T>
	{
		private Func<IContainer, T> _factory;

		public virtual bool IsSingleFactory
		{
			get { return false; }
		}

		public InjectionFactory(Func<IContainer, T> factory)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}
			_factory = factory;
		}

		public T Create(IContainer container)
		{
			return _factory(container);
		}
	}
}
