using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Storm.MvvmCross.Android.Target.Model;

namespace Storm.MvvmCross.Android.Target.Compiler
{
    internal partial class BindingLanguageParser
    {
        public BindingLanguageParser() : base(null) { }

        public Expression Parse(string s, out bool result)
        {
            byte[] inputBuffer = Encoding.Default.GetBytes(s);
            MemoryStream stream = new MemoryStream(inputBuffer);
            Scanner = new BindingLanguageScanner(stream);

            result = Parse();
	        if (result)
	        {
		        ProcessModeNodes(CurrentSemanticValue.Expression);
		        return CurrentSemanticValue.Expression;
	        }
	        return null;
        }

		// Replace all mode by ModeExpression instead of TextExpression
	    private void ProcessModeNodes(Expression expression)
	    {
		    if (expression.IsOfType(ExpressionType.Binding) && expression.Has(BindingExpression.MODE))
		    {
			    TextExpression text = expression.Get<TextExpression>(BindingExpression.MODE);
			    if (text != null)
			    {
				    BindingMode mode;
				    if (Enum.TryParse(text.Value, true, out mode))
				    {
					    expression.Replace(BindingExpression.MODE, new ModeExpression {Value = mode});
				    }
				    else
				    {
					    BindingPreprocess.Logger.LogError("Invalid value for Mode in binding Expression actual value is {0}", text.Value);
				    }
			    }
			    else
			    {
				    BindingPreprocess.Logger.LogError("Invalid value for Mode in binding expression actual value is of type {0}", expression[BindingExpression.MODE].Type.ToString());
			    }
		    }

		    foreach (Expression child in expression.Attributes.Values)
		    {
			    ProcessModeNodes(child);
		    }
	    }

	    private List<Tuple<string, Expression>> CreateAndAdd(string key, Expression value)
	    {
		    return new List<Tuple<string, Expression>>
		    {
			    new Tuple<string, Expression>(key, value),
		    };
	    }

	    private void CopyList(ValueType from, ValueType to)
	    {
		    if (to.KeyValueList == null)
		    {
			    to.KeyValueList = new List<Tuple<string, Expression>>();
		    }

		    if (from.KeyValueList != null)
		    {
			    to.KeyValueList.AddRange(from.KeyValueList);
		    }
	    }
    }
}
