using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm.Services
{
	public interface IMessageDialogService
	{
		void Show(string dialogKey);

		void Show(string dialogKey, Dictionary<string, object> parameters);
	}
}
