using System;
using Funq;

namespace Storm.Mvvm
{
	public class ContainerBase : IContainer, IDisposable
	{
		#region Fields

		private bool m_disposed = false;
		private Container m_container = null;

		#endregion

		#region Constructors

		public ContainerBase()
		{
			m_container = new Container();
			Initialize();
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
			m_container.Register<TClass>(_object);
		}

		public void RegisterInstance<TInterface, TClass>(TClass _object) where TClass : TInterface
		{
			m_container.Register<TInterface>(_object);
		}

		public void RegisterFactory<TClass>(Func<ContainerBase, TClass> _factory)
		{
			m_container.Register<TClass>((container) => _factory(this));
		}

		public void RegisterFactory<TInterface, TClass>(Func<ContainerBase, TClass> _factory) where TClass : TInterface
		{
			m_container.Register<TInterface>((container) => _factory(this));
		}

		public TClass Resolve<TClass>()
		{
			return m_container.Resolve<TClass>();
		}

		#endregion

		#region Protected methods

		protected virtual void Dispose(bool _disposing)
		{
			if(m_disposed)
			{
				return;
			}

			if(_disposing)
			{
				m_container.Dispose();
				Clean();
			}

			m_disposed = true;
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
