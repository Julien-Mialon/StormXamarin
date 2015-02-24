using System;

namespace Storm.Mvvm.Inject
{
	class InjectionLazyFactory<T> : InjectionFactory<T>
	{
		public InjectionLazyFactory(Func<IContainer, T> factory) : base(factory)
		{

		}

		public override bool IsSingleFactory
		{
			get { return true; }
		}
	}
}
