using System.CodeDom;

namespace Storm.Binding.AndroidTarget.Process
{
	public class BindingExpression
	{
		public enum BindingModes
		{
			OneWay,
			TwoWay
		}

		/* Mandatory fields */
		public string TargetObjectId { get; set; }

		public string TargetFieldId { get; set; }

		public string SourcePath { get; set; }

		/* Optional fields */
		public CodePropertyReferenceExpression ConverterReference { get; set; }

		// only if converter specified
		public string ConverterParameter { get; set; }

		public CodePropertyReferenceExpression ViewSelectorReference { get; set; }

		// only if a CommandParameter is specified
		public CodePropertyReferenceExpression CommandParameterReference { get; set; }

		// default value : OneWay
		public BindingModes Mode { get; set; }

		// only if Mode = TwoWay
		public string UpdateEvent { get; set; }

		public BindingExpression()
		{
			Mode = BindingModes.OneWay;
		}
	}
}
