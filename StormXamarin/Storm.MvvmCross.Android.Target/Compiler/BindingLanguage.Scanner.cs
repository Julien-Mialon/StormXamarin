namespace Storm.MvvmCross.Android.Target.Compiler
{
    internal partial class BindingLanguageScanner
    {

        void GetContent()
        {
	        string resultString = yytext;
	        if (resultString.StartsWith("'"))
	        {
		        // have to remove ' and simplify \'
		        resultString = resultString.Trim('\'');
		        resultString = resultString.Replace("\\'", "'");
	        }

	        yylval.Content = resultString;
        }

		public override void yyerror(string format, params object[] args)
		{
			base.yyerror(format, args);

			LexLog("ERROR => " + string.Format(format, args));
		}

	    void LexLog(string message)
	    {
		    BindingPreprocess.LexLog(message);
	    }
    }
}
