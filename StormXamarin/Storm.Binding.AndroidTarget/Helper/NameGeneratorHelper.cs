namespace Storm.Binding.AndroidTarget.Helper
{
	public static class NameGeneratorHelper
	{
		private const string VIEWHOLDER_FORMAT = "AutoGen_ViewHolder_{0}";
		private static int _viewHolderCounter;

		public static string GetViewHolderName()
		{
			return string.Format(VIEWHOLDER_FORMAT, _viewHolderCounter++);
		}
	}
}
