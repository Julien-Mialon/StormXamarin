using System;

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
