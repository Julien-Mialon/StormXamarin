using System;
using Foundation;

namespace Storm.Localization
{
	internal static class LocalizedStrings
	{
		public static string Test
		{
			get
			{
				return NSBundle.MainBundle.LocalizedString("Test", null);
			}
		}
		public static string Text__Text
		{
			get
			{
				return NSBundle.MainBundle.LocalizedString("Text__Text", null);
			}
		}
		public static string Title__Text
		{
			get
			{
				return NSBundle.MainBundle.LocalizedString("Title__Text", null);
			}
		}
	}
}
