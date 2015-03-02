namespace Storm.Mvvm.TemplateSelectors
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
