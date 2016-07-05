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
	internal sealed class ServiceEntry<TService, TFunc> : ServiceEntry, IRegistration<TService>
	{
		public ServiceEntry(TFunc factory)
		{
			this.Factory = factory;
		}

		/// <summary>
		/// The Func delegate that creates instances of the service.
		/// </summary>
		public TFunc Factory;

		/// <summary>
		/// The cached service instance if the scope is <see cref="ReuseScope.Hierarchy"/> or 
		/// <see cref="ReuseScope.Container"/>.
		/// </summary>
		internal TService Instance;

		/// <summary>
		/// The Func delegate that initializes the object after creation.
		/// </summary>
		internal Action<Container, TService> Initializer;

		internal void InitializeInstance(TService instance)
		{
			// Save instance if Hierarchy or Container Reuse 
			if (Reuse != ReuseScope.None)
				Instance = instance;

			// Track for disposal if necessary
			if (Owner == Owner.Container && instance is IDisposable)
				Container.TrackDisposable(instance);

			// Call initializer if necessary
			if (Initializer != null)
				Initializer(Container, instance);
		}

		public IReusedOwned InitializedBy(Action<Container, TService> initializer)
		{
			this.Initializer = initializer;
			return this;
		}

		/// <summary>
		/// Clones the service entry assigning the <see cref="Container"/> to the 
		/// <paramref name="newContainer"/>. Does not copy the <see cref="Instance"/>.
		/// </summary>
		public ServiceEntry<TService, TFunc> CloneFor(Container newContainer)
		{
			return new ServiceEntry<TService, TFunc>(Factory)
			{
				Owner = Owner,
				Reuse = Reuse,
				Container = newContainer,
				Initializer = Initializer,
			};
		}
	}
}
