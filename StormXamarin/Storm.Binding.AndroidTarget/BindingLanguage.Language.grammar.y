%namespace Storm.Binding.AndroidTarget
%partial
%parsertype BindingLanguageParser
%visibility internal
%tokentype Token

%union { 
			public string content; 
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

expression				: OPEN_BRACKET expressionKeyword expressionContent CLOSE_BRACKET
						;

expressionKeyword		: BINDING
						| RESOURCE
						| TRANSLATION
						;

expressionContent		: 
						| IDENTIFIER
						| IDENTIFIER COMMA dualContent
						| dualContent
						;

dualContent				: IDENTIFIER EQUAL values
						| IDENTIFIER EQUAL values COMMA dualContent
						;

values					: IDENTIFIER
						| expression
						;
%%