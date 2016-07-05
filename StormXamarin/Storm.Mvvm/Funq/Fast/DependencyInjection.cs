using System;

#pragma warning disable 1589

namespace Funq.Fast
{
    /// <include file='Funq.xdoc' path='docs/doc[@for="Container"]/*'/>
    public static class DependencyInjection
    {
        private static readonly Container Container;

        static DependencyInjection()
        {
            Container = new Container();
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.DefaultOwner"]/*'/>
        public static Owner DefaultOwner
        {
            get { return Container.DefaultOwner; }
            set { Container.DefaultOwner = value; }
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.DefaultReuse"]/*'/>
        public static ReuseScope DefaultReuse
        {
            get { return Container.DefaultReuse; }
            set { Container.DefaultReuse = value; }
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.CreateChildContainer"]/*'/>
        public static Container CreateChildContainer()
        {
            return Container.CreateChildContainer();
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService}(factory)"]/*'/>
        public static IRegistration<TService> Register<TService>(Func<Container, TService> factory)
        {
            return Container.Register(factory);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService}(name, factory)"]/*'/>
        public static IRegistration<TService> Register<TService>(string name, Func<Container, TService> factory)
        {
            return Container.Register(name, factory);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService, TArg}(name, factory)"]/*'/>
        public static IRegistration<TService> Register<TService, TArg>(string name, Func<Container, TArg, TService> factory)
        {
            return Container.Register(name, factory);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register{TService, TArg}(factory)"]/*'/>
        public static IRegistration<TService> Register<TService, TArg>(Func<Container, TArg, TService> factory)
        {
            return Container.Register(factory);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register(instance)"]/*'/>
        public static void Register<TService>(TService instance)
        {
            Container.Register(instance);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Register(name, instance)"]/*'/>
        public static void Register<TService>(string name, TService instance)
        {
            Container.Register(name, instance);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Resolve{TService}"]/*'/>
        public static TService Resolve<TService>()
        {
            return Container.Resolve<TService>();
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.Resolve{TService, TArg}"]/*'/>
        public static TService Resolve<TService, TArg>(TArg arg)
        {
            return Container.Resolve<TService, TArg>(arg);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.ResolveNamed{TService}"]/*'/>
        public static TService ResolveNamed<TService>(string name)
        {
            return Container.ResolveNamed<TService>(name);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.ResolveNamed{TService, TArg}"]/*'/>
        public static TService ResolveNamed<TService, TArg>(string name, TArg arg)
        {
            return Container.ResolveNamed<TService, TArg>(name, arg);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService}"]/*'/>
        public static Func<TService> LazyResolve<TService>()
        {
            return Container.LazyResolve<TService>();
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, TArgs}"]/*'/>
        public static Func<TArg, TService> LazyResolve<TService, TArg>()
        {
            return Container.LazyResolve<TService, TArg>();
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, name}"]/*'/>
        public static Func<TService> LazyResolveNamed<TService>(string name)
        {
            return Container.LazyResolveNamed<TService>(name);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, TArgs, name}"]/*'/>
        public static Func<TArg, TService> LazyResolveNamed<TService, TArg>(string name)
        {
            return Container.LazyResolveNamed<TService, TArg>(name);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolve{TService}"]/*'/>
        public static TService TryResolve<TService>()
        {
            return Container.TryResolve<TService>();
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolve{TService, TArg}"]/*'/>
        public static TService TryResolve<TService, TArg>(TArg arg)
        {
            return Container.TryResolve<TService, TArg>(arg);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolveNamed{TService}"]/*'/>
        public static TService TryResolveNamed<TService>(string name)
        {
            return Container.TryResolveNamed<TService>(name);
        }

        /// <include file='Funq.xdoc' path='docs/doc[@for="Container.TryResolveNamed{TService, TArg}"]/*'/>
        public static TService TryResolveNamed<TService, TArg>(string name, TArg arg)
        {
            return Container.TryResolveNamed<TService, TArg>(name, arg);
        }
    }
}
#pragma warning restore 1589