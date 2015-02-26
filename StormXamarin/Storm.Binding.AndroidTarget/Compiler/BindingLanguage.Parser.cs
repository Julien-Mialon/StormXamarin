using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Storm.Binding.AndroidTarget.Compiler
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
	        return result ? CurrentSemanticValue.Expression : null;
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
