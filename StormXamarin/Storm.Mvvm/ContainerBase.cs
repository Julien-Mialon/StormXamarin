using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			this.m_container = new Container();
			this.Initialize();
		}

		#endregion

		#region Public methods

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void RegisterInstance<TClass>(TClass _object)
		{
			this.m_container.Register<TClass>(_object);
		}

		public void RegisterInstance<TInterface, TClass>(TClass _object) where TClass : TInterface
		{
			this.m_container.Register<TInterface>(_object);
		}

		public void RegisterFactory<TClass>(Func<ContainerBase, TClass> _factory)
		{
			this.m_container.Register<TClass>((container) => _factory(this));
		}

		public void RegisterFactory<TInterface, TClass>(Func<ContainerBase, TClass> _factory) where TClass : TInterface
		{
			this.m_container.Register<TInterface>((container) => _factory(this));
		}

		public TClass Resolve<TClass>()
		{
			return this.m_container.Resolve<TClass>();
		}

		#endregion

		#region Protected methods

		protected virtual void Dispose(bool _disposing)
		{
			if(this.m_disposed)
			{
				return;
			}

			if(_disposing)
			{
				this.m_container.Dispose();
				this.Clean();
			}

			this.m_disposed = true;
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
