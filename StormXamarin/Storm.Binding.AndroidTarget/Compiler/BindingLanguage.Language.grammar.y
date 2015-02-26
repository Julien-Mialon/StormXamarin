%namespace Storm.Binding.AndroidTarget.Compiler
%partial
%parsertype BindingLanguageParser
%visibility internal
%tokentype Token

%union 
{ 
	public string Content; 
	public Expression Expression;
	public ExpressionType ExpressionType;
	public List<Tuple<string, Expression>> KeyValueList;
}

%start expression

%token OPEN_BRACKET
%token CLOSE_BRACKET
%token COMMA
%token EQUAL
%token BINDING
%token RESOURCE
%token TRANSLATION
%token IDENTIFIER


%%

expression				: OPEN_BRACKET expressionKeyword expressionContent CLOSE_BRACKET { $$.Expression = ExpressionFactory.CreateExpression($2.ExpressionType, $3.KeyValueList); }
						;

expressionKeyword		: BINDING		{ $$.ExpressionType = ExpressionType.Binding; }
						| RESOURCE		{ $$.ExpressionType = ExpressionType.Resource; }
						| TRANSLATION	{ $$.ExpressionType = ExpressionType.Translation; }
						;

expressionContent		: 
						| values							{ $$.KeyValueList = CreateAndAdd(null, $1.Expression); }
						| values COMMA dualContent			{ $$.KeyValueList = CreateAndAdd(null, $1.Expression); CopyList($3, $$); }
						| dualContent						{ CopyList($1, $$); }
						;

dualContent				: IDENTIFIER EQUAL values						{ $$.KeyValueList = CreateAndAdd($1.Content, $3.Expression); }
						| IDENTIFIER EQUAL values COMMA dualContent		{ $$.KeyValueList = CreateAndAdd($1.Content, $3.Expression); CopyList($5, $$); }
						;

values					: IDENTIFIER	{ $$.Expression = ExpressionFactory.CreateTextExpression($1.Content); }
						| expression	{ $$.Expression = $1.Expression; }
						;
%%