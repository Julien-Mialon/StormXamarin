using System.Windows.Data;

namespace Storm.Mvvm.Bindings
{
	public class BindingExpression
	{
		public string TargetField { get; set; }

		public string SourcePath { get; set; }

		public IValueConverter Converter { get; set; }

		public object ConverterParameter { get; set; }

		public BindingMode Mode { get; set; }

		public string UpdateEvent { get; set; }

		public BindingTargetType TargetType { get; set; }

		public CommandParameterProxy CommandParameter { get; set; }

		public BindingExpression()
		{
			
		}

		public BindingExpression(string targetField, string sourcePath)
		{
			TargetField = targetField;
			SourcePath = sourcePath;
		}
	}
}
