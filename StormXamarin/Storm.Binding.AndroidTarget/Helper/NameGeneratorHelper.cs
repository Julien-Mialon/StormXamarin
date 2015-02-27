namespace Storm.Binding.AndroidTarget.Helper
{
	public static class NameGeneratorHelper
	{
		private const string VIEWHOLDER_FORMAT = "Generated_ViewHolder_{0}";
		private const string FIELD_FORMAT = "_generated_field_{0}";
		private const string RESOURCE_FORMAT = "Generated_Resource_{0}";
		private const string COMMAND_PARAMETER_FORMAT = "Generated_CommandParameter_{0}";
		private const string OBJECT_FORMAT = "bindingObject_{0}";
		private const string EXPRESSION_FORMAT = "bindingExpression_{0}";
		
		
		private const string ADAPTER_INTERNAL_NAME = "adapter";
		private const string VIEW_SELECTOR_INTERNAL_NAME = "viewSelector";

		private static int _viewHolderCounter;
		private static int _fieldCounter;
		private static int _resourceCounter;
		private static int _objectCounter;
		private static int _expressionCounter;
		private static int _commandParameterCounter;

		public static string GetViewHolderName()
		{
			return string.Format(VIEWHOLDER_FORMAT, _viewHolderCounter++);
		}

		public static string GetFieldName()
		{
			return string.Format(FIELD_FORMAT, _fieldCounter++);
		}

		public static string GetResourceName()
		{
			return string.Format(RESOURCE_FORMAT, _resourceCounter++);
		}

		public static string GetCommandParameterName()
		{
			return string.Format(COMMAND_PARAMETER_FORMAT, _commandParameterCounter++);
		}

		public static string GetBindingObjectName()
		{
			return string.Format(OBJECT_FORMAT, _objectCounter++);
		}

		public static string GetBindingExpressionName()
		{
			return string.Format(EXPRESSION_FORMAT, _expressionCounter++);
		}
	}
}
