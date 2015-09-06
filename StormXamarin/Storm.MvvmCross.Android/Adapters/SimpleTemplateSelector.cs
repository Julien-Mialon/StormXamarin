namespace Storm.MvvmCross.Android.Adapters
{
	public class SimpleTemplateSelector : AbstractTemplateSelector
	{
		public DataTemplate Template { get; set; }

		public override DataTemplate GetTemplate(object model)
		{
			return Template;
		}
	}
}
