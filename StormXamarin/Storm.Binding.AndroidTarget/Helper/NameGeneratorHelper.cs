namespace Storm.Binding.AndroidTarget.Helper
{
	public static class NameGeneratorHelper
	{
		public const string SETUP_RESOURCES_NAME = "SetupResources";

		private const string VIEW_ID_FORMAT = "Generated_ViewElement_{0}";
		private const string VIEWHOLDER_FORMAT = "Generated_ViewHolder_{0}";
		private const string FIELD_FORMAT = "_generated_field_{0}";
		private const string RESOURCE_KEY_FORMAT = "Generated_Resource_Key_{0}";
		private const string RESOURCE_FORMAT = "Generated_Resource_{0}";
		private const string COMMAND_PARAMETER_FORMAT = "Generated_CommandParameter_{0}";
		private const string OBJECT_FORMAT = "generated_bindingObject_{0}";
		private const string EXPRESSION_FORMAT = "generated_bindingExpression_{0}";
		
		private const string ADAPTER_INTERNAL_NAME = "adapter";
		private const string VIEW_SELECTOR_INTERNAL_NAME = "viewSelector";

		private static int _viewObjectId;
		private static int _viewHolderCounter;
		private static int _fieldCounter;
		private static int _resourceKeyCounter;
		private static int _resourceCounter;
		private static int _objectCounter;
		private static int _expressionCounter;
		private static int _commandParameterCounter;

		public static string GetViewId()
		{
			return string.Format(VIEW_ID_FORMAT, _viewObjectId++);
		}

		public static string GetViewHolderName()
		{
			return string.Format(VIEWHOLDER_FORMAT, _viewHolderCounter++);
		}

		public static string GetFieldName()
		{
			return string.Format(FIELD_FORMAT, _fieldCounter++);
		}

		public static string GetResourceKey()
		{
			return string.Format(RESOURCE_KEY_FORMAT, _resourceKeyCounter++);
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
