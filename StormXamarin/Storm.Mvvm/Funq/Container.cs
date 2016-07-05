#region License
// Microsoft Public License (Ms-PL)
// 
// This license governs use of the accompanying software. If you use the software, you accept this license. If you do not accept the license, do not use the software.
// 
// 1. Definitions
// 
//   The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under U.S. copyright law.
// 
//   A "contribution" is the original software, or any additions or changes to the software.
// 
//   A "contributor" is any person that distributes its contribution under this license.
// 
//   "Licensed patents" are a contributor's patent claims that read directly on its contribution.
// 
// 2. Grant of Rights
// 
//   (A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
// 
//   (B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.
// 
// 3. Conditions and Limitations
// 
//   (A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
// 
//   (B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, your patent license from such contributor to the software ends automatically.
// 
//   (C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution notices that are present in the software.
// 
//   (D) If you distribute any portion of the software in source code form, you may do so only under this license by including a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object code form, you may only do so under a license that complies with this license.
// 
//   (E ) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular purpose and non-infringement.
#endregion

using System;
using System.Collections.Generic;

namespace Funq
{
	/// <include file='Funq.xdoc' path='docs/doc[@for="Container"]/*'/>
	public sealed partial class Container : IDisposable
	{
		Dictionary<ServiceKey, ServiceEntry> services = new Dictionary<ServiceKey, ServiceEntry>();
		// Disposable components include factory-scoped instances that we don't keep 
		// a strong reference to. 
		Stack<WeakReference> disposables = new Stack<WeakReference>();
		// We always hold a strong reference to child containers.
		Stack<Container> childContainers = new Stack<Container>();
		Container parent;

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.ctor"]/*'/>
		public Container()
		{
			services[new ServiceKey(typeof(Func<Container, Container>), null)] =
				new ServiceEntry<Container, Func<Container, Container>>((Func<Container, Container>)(c => c))
				{
					Container = this,
					Instance = this,
					Owner = Owner.External,
					Reuse = ReuseScope.Container,
				};
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.DefaultOwner"]/*'/>
		public Owner DefaultOwner { get; set; }

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.DefaultReuse"]/*'/>
		public ReuseScope DefaultReuse { get; set; }

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.CreateChildContainer"]/*'/>
		public Container CreateChildContainer()
		{
			var child = new Container { parent = this };
			childContainers.Push(child);
			return child;
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Dispose"]/*'/>
		public void Dispose()
		{
			while (disposables.Count > 0)
			{
				var wr = disposables.Pop();
				var disposable = (IDisposable)wr.Target;
				if (wr.IsAlive)
					disposable.Dispose();
			}
			while (childContainers.Count > 0)
			{
				childContainers.Pop().Dispose();
			}
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService}(factory)"]/*'/>
		public IRegistration<TService> Register<TService>(Func<Container, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService}(name, factory)"]/*'/>
		public IRegistration<TService> Register<TService>(string name, Func<Container, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, TService>>(name, factory);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService, TArg}(name, factory)"]/*'/>
		public IRegistration<TService> Register<TService, TArg>(string name, Func<Container, TArg, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, TArg, TService>>(name, factory);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService, TArg}(factory)"]/*'/>
		public IRegistration<TService> Register<TService, TArg>(Func<Container, TArg, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register(instance)"]/*'/>
		public void Register<TService>(TService instance)
		{
			Register(null, instance);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register(name, instance)"]/*'/>
		public void Register<TService>(string name, TService instance)
		{
			var entry = RegisterImpl<TService, Func<Container, TService>>(name, null);

			// Set sensible defaults for instance registration.
			entry.ReusedWithin(ReuseScope.Hierarchy).OwnedBy(Owner.External);
			entry.InitializeInstance(instance);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Resolve{TService}"]/*'/>
		public TService Resolve<TService>()
		{
			return ResolveNamed<TService>(null);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.Resolve{TService, TArg}"]/*'/>
		public TService Resolve<TService, TArg>(TArg arg)
		{
			return ResolveNamed<TService, TArg>(null, arg);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.ResolveNamed{TService}"]/*'/>
		public TService ResolveNamed<TService>(string name)
		{
			return ResolveImpl<TService>(name, true);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.ResolveNamed{TService, TArg}"]/*'/>
		public TService ResolveNamed<TService, TArg>(string name, TArg arg)
		{
			return ResolveImpl<TService, TArg>(name, true, arg);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService}"]/*'/>
		public Func<TService> LazyResolve<TService>()
		{
			return LazyResolveNamed<TService>(null);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, TArgs}"]/*'/>
		public Func<TArg, TService> LazyResolve<TService, TArg>()
		{
			return LazyResolveNamed<TService, TArg>(null);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, name}"]/*'/>
		public Func<TService> LazyResolveNamed<TService>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, TService>>(name);
			return () => ResolveNamed<TService>(name);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, TArgs, name}"]/*'/>
		public Func<TArg, TService> LazyResolveNamed<TService, TArg>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, TArg, TService>>(name);
			return arg => ResolveNamed<TService, TArg>(name, arg);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolve{TService}"]/*'/>
		public TService TryResolve<TService>()
		{
			return TryResolveNamed<TService>(null);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolve{TService, TArg}"]/*'/>
		public TService TryResolve<TService, TArg>(TArg arg)
		{
			return TryResolveNamed<TService, TArg>(null, arg);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolveNamed{TService}"]/*'/>
		public TService TryResolveNamed<TService>(string name)
		{
			return ResolveImpl<TService>(name, false);
		}

		/// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolveNamed{TService, TArg}"]/*'/>
		public TService TryResolveNamed<TService, TArg>(string name, TArg arg)
		{
			return ResolveImpl<TService, TArg>(name, false, arg);
		}

		/// <summary>
		/// Tracks the disposable object, which can be a container-owned instance, or a transient factory-created 
		/// one.
		/// </summary>
		internal void TrackDisposable(object instance)
		{
			disposables.Push(new WeakReference(instance));
		}

		private ServiceEntry<TService, TFunc> RegisterImpl<TService, TFunc>(string name, TFunc factory)
		{
			if (typeof(TService) == typeof(Container))
				throw new ArgumentException("Container service is built -in and read - only.");

			var entry = new ServiceEntry<TService, TFunc>(factory)
			{
				Container = this,
				Reuse = DefaultReuse,
				Owner = DefaultOwner
			};
			var key = new ServiceKey(typeof(TFunc), name);

			services[key] = entry;

			return entry;
		}

		private TService ResolveImpl<TService>(string name, bool throwIfMissing)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, TService>>(name, throwIfMissing);

			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, TArg>(string name, bool throwIfMissing, TArg arg)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, TArg, TService>>(name, throwIfMissing);

			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private ServiceEntry<TService, TFunc> GetEntry<TService, TFunc>(string serviceName, bool throwIfMissing)
		{
			var key = new ServiceKey(typeof(TFunc), serviceName);
			ServiceEntry entry = null;
			Container container = this;

			// Go up the hierarchy always for registrations.
			while (!container.services.TryGetValue(key, out entry) && container.parent != null)
			{
				container = container.parent;
			}

			if (entry != null)
			{
				if (entry.Reuse == ReuseScope.Container && entry.Container != this)
				{
					entry = ((ServiceEntry<TService, TFunc>)entry).CloneFor(this);
					services[key] = entry;
				}
			}
			else if (throwIfMissing)
			{
				ThrowMissing<TService>(serviceName);
			}

			return (ServiceEntry<TService, TFunc>)entry;
		}

		private static TService ThrowMissing<TService>(string serviceName)
		{
			if (serviceName == null)
				throw new ResolutionException(typeof(TService));
			else
				throw new ResolutionException(typeof(TService), serviceName);
		}

		private void ThrowIfNotRegistered<TService, TFunc>(string name)
		{
			GetEntry<TService, TFunc>(name, true);
		}
	}
}
