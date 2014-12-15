using System;

namespace Storm.Mvvm.Dialogs
{
	[Flags]
	public enum DialogsButton
	{
		None = 0,
		Positive = 1,
		Neutral = 2,
		Negative = 4,
	}
}
