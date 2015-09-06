using Cirrious.CrossCore.Converters;
using Storm.MvvmCross.Bindings.Internal;

namespace Storm.MvvmCross.Bindings
{
	public class BindingExpression
	{
		public string TargetField { get; set; }

		public string SourcePath { get; set; }

		public IMvxValueConverter Converter { get; set; }

		public object ConverterParameter { get; set; }

		public BindingMode Mode { get; set; }

		public string UpdateEvent { get; set; }

		public BindingExpressionTargetFieldType TargetType { get; set; }

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
