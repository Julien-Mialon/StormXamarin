using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Storm.Mvvm.Services
{
	public interface ICurrentPageService
	{
		Page CurrentPage { get; }

		void Push(Page newPage);

		void Pop();
	}
}
