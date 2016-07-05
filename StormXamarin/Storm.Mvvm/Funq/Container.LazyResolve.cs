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

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, TService> LazyResolve<TService, T1, T2>()
		{
			return LazyResolveNamed<TService, T1, T2>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, TService> LazyResolveNamed<TService, T1, T2>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, TService>>(name);
			return (arg1, arg2) => ResolveNamed<TService, T1, T2>(name, arg1, arg2);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, TService> LazyResolve<TService, T1, T2, T3>()
		{
			return LazyResolveNamed<TService, T1, T2, T3>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, TService> LazyResolveNamed<TService, T1, T2, T3>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, TService>>(name);
			return (arg1, arg2, arg3) => ResolveNamed<TService, T1, T2, T3>(name, arg1, arg2, arg3);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, TService> LazyResolve<TService, T1, T2, T3, T4>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, TService> LazyResolveNamed<TService, T1, T2, T3, T4>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, TService>>(name);
			return (arg1, arg2, arg3, arg4) => ResolveNamed<TService, T1, T2, T3, T4>(name, arg1, arg2, arg3, arg4);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, TService> LazyResolve<TService, T1, T2, T3, T4, T5>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5) => ResolveNamed<TService, T1, T2, T3, T4, T5>(name, arg1, arg2, arg3, arg4, arg5);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6>(name, arg1, arg2, arg3, arg4, arg5, arg6);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolve{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService> LazyResolve<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
		{
			return LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(null);
		}

		/// <include file='Funq.Overloads.xdoc' path='docs/doc[@for="Container.LazyResolveNamed{TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14}"]/*'/>
		[DebuggerStepThrough]
		public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService> LazyResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string name)
		{
			ThrowIfNotRegistered<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService>>(name);
			return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) => ResolveNamed<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(name, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
		}

	}
}
