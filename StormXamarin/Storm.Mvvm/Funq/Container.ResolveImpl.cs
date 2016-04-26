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

namespace Funq
{
	sealed partial class Container
	{
		/* All ResolveImpl are essentially equal, except for the type of the factory 
		 * which is "hardcoded" in each implementation. This slight repetition of 
		 * code gives us a bit more of perf. gain by avoiding an intermediate 
		 * func/lambda to call in a generic way as we did before.
		 */

		private TService ResolveImpl<TService, T1, T2>(string name, bool throwIfMissing, T1 arg1, T2 arg2)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

		private TService ResolveImpl<TService, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string name, bool throwIfMissing, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
		{
			// Would throw if missing as appropriate.
			var entry = GetEntry<TService, Func<Container, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TService>>(name, throwIfMissing);
			// Return default if not registered and didn't throw above.
			if (entry == null)
				return default(TService);

			TService instance = entry.Instance;
			if (instance == null)
			{
				instance = entry.Factory(entry.Container, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
				entry.InitializeInstance(instance);
			}

			return instance;
		}

	}
}
