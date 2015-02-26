namespace Storm.Binding.AndroidTarget.Compiler
{
	public class BindingExpression : Expression
	{
		public const string PATH = "Path";
		public const string CONVERTER = "Converter";
		public const string CONVERTER_PARAMETER = "ConverterParameter";
		public const string MODE = "Mode";
		public const string UPDATE_EVENT = "UpdateEvent";
		public const string TEMPLATE = "Template";
		public const string TEMPLATE_SELECTOR = "TemplateSelector";

		public override ExpressionType Type
		{
			get { return ExpressionType.Binding; }
		}

		protected override string ContentKey
		{
			get { return PATH; }
		}

		protected override string[] InternalAvailableKeys
		{
			get { return new[] { PATH, CONVERTER, CONVERTER_PARAMETER, MODE, UPDATE_EVENT, TEMPLATE, TEMPLATE_SELECTOR }; }
		}
	}
}