using System;
using Funq;

namespace Storm.Mvvm.Inject
{
	public class ContainerBase : IContainer, IDisposable
	{
		#region Fields

		private bool _disposed = false;
		private readonly Container _container = null;

		#endregion

		#region Constructors

		public ContainerBase()
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

		public void RegisterFactory<TClass>(Func<ContainerBase, TClass> factory)
		{
			_container.Register(container => factory(this));
		}

		public void RegisterFactory<TInterface, TClass>(Func<ContainerBase, TClass> factory) where TClass : TInterface
		{
			_container.Register<TInterface>(container => factory(this));
		}

		public TClass Resolve<TClass>()
		{
			return _container.Resolve<TClass>();
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

		public virtual void Initialize()
		{
			
		}

		protected virtual void Clean()
		{

		}

		#endregion
	}
}
