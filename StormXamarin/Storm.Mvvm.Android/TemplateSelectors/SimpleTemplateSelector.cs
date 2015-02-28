using Android.Views;

namespace Storm.Mvvm.TemplateSelectors
{
	public class SimpleTemplateSelector : AbstractTemplateSelector
	{
		public int Template { get; set; }

		public SimpleTemplateSelector(LayoutInflater layoutInflater) : base(layoutInflater)
		{

		}

		public override int GetViewId(object model)
		{
			return Template;
		}
	}
}
