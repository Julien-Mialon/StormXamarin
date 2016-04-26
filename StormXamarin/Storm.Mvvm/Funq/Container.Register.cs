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
using System.Diagnostics;

namespace Funq
{
	partial class Container
	{
		/* Contain just the typed overloads that are just pass-through to the real implementations.
		 * They all have DebuggerStepThrough to ease debugging. */

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2>(Func<Container, T1, T2, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2>(string name, Func<Container, T1, T2, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3>(Func<Container, T1, T2, T3, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3>(string name, Func<Container, T1, T2, T3, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4>(Func<Container, T1, T2, T3, T4, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4>(string name, Func<Container, T1, T2, T3, T4, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5>(Func<Container, T1, T2, T3, T4, T5, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5>(string name, Func<Container, T1, T2, T3, T4, T5, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6>(Func<Container, T1, T2, T3, T4, T5, T6, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6>(string name, Func<Container, T1, T2, T3, T4, T5, T6, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7>(Func<Container, T1, T2, T3, T4, T5, T6, T7, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService>>(name, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService> factory)
		{
			return Register(null, factory);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.Register{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}(name, factory)"]/*'/>
		[DebuggerStepThrough]
		public IRegistration<TService> Register<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string name, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService> factory)
		{
			return RegisterImpl<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService>>(name, factory);
		}

	}
}
