using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm.Inject
{
	public interface IInjectionFactory
	{

	}

	public interface IInjectionFactory<out T> : IInjectionFactory
	{
		T Create(IContainer container);
	}
}
