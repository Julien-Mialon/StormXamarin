using System;

namespace Storm.Mvvm
{
	public interface IContainer
	{
		void RegisterFactory<TClass>(Func<ContainerBase, TClass> _factory);
		
		void RegisterFactory<TInterface, TClass>(Func<ContainerBase, TClass> _factory) where TClass : TInterface;
		
		void RegisterInstance<TClass>(TClass _object);
		
		void RegisterInstance<TInterface, TClass>(TClass _object) where TClass : TInterface;
		
		TClass Resolve<TClass>();
	}
}
