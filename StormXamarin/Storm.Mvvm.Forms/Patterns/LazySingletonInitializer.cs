using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Storm.Mvvm.Patterns
{
	public class LazySingletonInitializer<T> where T : class
	{
		public static T Value = DependencyService.Get<T>();
	}
}
