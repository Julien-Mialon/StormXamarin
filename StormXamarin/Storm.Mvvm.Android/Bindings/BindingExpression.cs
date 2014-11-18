using System.Windows.Data;

namespace Storm.Mvvm.Android.Bindings
{
	public class BindingExpression
	{
		public string TargetField { get; set; }

		public string SourcePath { get; set; }

		public IValueConverter Converter { get; set; }

		public string ConverterParameter { get; set; }

		public BindingMode Mode { get; set; }

		public string UpdateEvent { get; set; }

		public BindingExpression()
		{
			
		}

		public BindingExpression(string targetField, string sourcePath)
		{
			this.TargetField = targetField;
			this.SourcePath = sourcePath;
		}
	}
}
