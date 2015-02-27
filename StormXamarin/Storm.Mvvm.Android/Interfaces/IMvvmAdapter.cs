namespace Storm.Mvvm.Interfaces
{
	public interface IMvvmAdapter
	{
		object Collection { get; set; }

		ITemplateSelector TemplateSelector { get; set; }
	}
}