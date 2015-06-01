using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storm.Mvvm.Navigation
{
	public interface IMvvmDialog
	{
		string ParametersKey { get; set; }

		void Dismiss();

		void Show();

		event EventHandler Dismissed;
	}
}
