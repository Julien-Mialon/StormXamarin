using System;
using System.Collections.Generic;
using Funq;

namespace Storm.Mvvm.Inject
{
	public class ContainerBase : IContainer, IDisposable
	{
		#region Fields

		private bool _disposed = false;
		private readonly Container _container = null;

		private Dictionary<Type, IInjectionFactory> _factories = new Dictionary<Type, IInjectionFactory>(); 

		#endregion

		#region Constructors

		protected ContainerBase()
		{
			_container = new Container();
		}

		#endregion

		#region Public methods

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void RegisterInstance<TClass>(TClass _object)
		{
			_container.Register(_object);
		}

		public void RegisterInstance<TInterface, TClass>(TClass _object) where TClass : TInterface
		{
			_container.Register<TInterface>(_object);
		}

		public void RegisterFactory<TClass>(Func<IContainer, TClass> factory)
		{
			IInjectionFactory<TClass> objectFactory = new InjectionFactory<TClass>(factory);
			_factories.Add(typeof(TClass), objectFactory);
		}

		public void RegisterFactory<TInterface, TClass>(Func<IContainer, TClass> factory) where TClass : TInterface
		{
			IInjectionFactory<TClass> objectFactory = new InjectionFactory<TClass>(factory);
			_factories.Add(typeof(TInterface), objectFactory);
		}

		public TClass Resolve<TClass>()
		{
			if (!_factories.ContainsKey(typeof (TClass)))
			{
				return _container.Resolve<TClass>();
			}

			IInjectionFactory<TClass> factory = _factories[typeof (TClass)] as IInjectionFactory<TClass>;
			if (factory == null)
			{
				throw new ArgumentException("TClass : factory = null");
			}
			return factory.Create(this);
		}

		#endregion

		#region Protected methods

		protected virtual void Dispose(bool disposing)
		{
			if(_disposed)
			{
				return;
			}

			if(disposing)
			{
				_container.Dispose();
				Clean();
			}

			_disposed = true;
		}

		protected virtual void Initialize()
		{
			
		}

		protected virtual void Clean()
		{

		}

		#endregion
	}
}
