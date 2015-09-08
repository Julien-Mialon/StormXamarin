using System.Collections.Generic;
using Storm.MvvmCross.Android.Target.Model;

namespace Storm.MvvmCross.Android.Target.Compiler
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
		public const string ADAPTER = "Adapter";

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
			get { return new[] { PATH, CONVERTER, CONVERTER_PARAMETER, MODE, UPDATE_EVENT, TEMPLATE, TEMPLATE_SELECTOR, ADAPTER }; }
		}

		protected override string[] InternalOverridableKeys
		{
			get { return new[] { PATH }; }
		}

		public BindingExpression()
		{
			//Set default value for Path
			Add(PATH, new TextExpression { Value = "" });
		}

		protected override Dictionary<string, IEnumerable<ExpressionType>> GetExpectedValueType()
		{
			return new Dictionary<string, IEnumerable<ExpressionType>>
			{
				{PATH, new List<ExpressionType> {ExpressionType.Value}},
				{CONVERTER, new List<ExpressionType> {ExpressionType.Binding, ExpressionType.Resource}},
				{CONVERTER_PARAMETER, new List<ExpressionType> {ExpressionType.Binding, ExpressionType.Resource, ExpressionType.Translation, ExpressionType.Value}},
				{MODE, new List<ExpressionType> {ExpressionType.BindingMode}},
				{UPDATE_EVENT, new List<ExpressionType> {ExpressionType.Value}},
				{TEMPLATE, new List<ExpressionType> {ExpressionType.Resource}},
				{TEMPLATE_SELECTOR, new List<ExpressionType> {ExpressionType.Resource}},
				{ADAPTER, new List<ExpressionType> {ExpressionType.Resource}},
			};
		}

		protected override bool CheckConstraints()
		{
			if (Has(CONVERTER_PARAMETER) && !Has(CONVERTER))
			{
				BindingPreprocess.Logger.LogError("A binding expression can not have a ConverterParameter without a Converter");
				return false;
			}

			if (Has(MODE))
			{
				BindingMode mode = Get<ModeExpression>(MODE).Value;

				if (Has(UPDATE_EVENT) && mode == BindingMode.OneWay)
				{
					BindingPreprocess.Logger.LogError("Binding expression with mode OneWay can not have an UpdateEvent");
					return false;
				}
				if (!Has(UPDATE_EVENT) && (mode == BindingMode.OneWayToSource || mode == BindingMode.TwoWay))
				{
					BindingPreprocess.Logger.LogError("Binding expression with mode TwoWay or OneWayToSource need to have an UpdateEvent");
					return false;
				}
			}

			if (Has(TEMPLATE) && Has(TEMPLATE_SELECTOR))
			{
				BindingPreprocess.Logger.LogError("Binding expression can not have Template & TemplateSelector");
				return false;
			}

			if (Has(ADAPTER) && (Has(TEMPLATE) || Has(TEMPLATE_SELECTOR)))
			{
				BindingPreprocess.Logger.LogError("Binding expression can not have Adapter if they have a Template or a TemplateSelector");
				return false;
			}
			return true;
		}
	}
}