%namespace Storm.Binding.AndroidTarget
%scannertype BindingLanguageScanner
%visibility internal
%tokentype Token

%option stack, minimize, parser, verbose, persistbuffer, noembedbuffers


OpenBracket				\{
CloseBracket			\}
Comma					,
Equal					=

BindingKeyword			Binding
ResourceKeyword			Resource
TranslationKeyword		Translation

Identifier				([a-zA-Z0-9_\-.]+)|('([^'"]*(\\')?)*')

Eol             (\r\n?|\n)
Space           [ \t]

%{

%}

%%

/* Scanner body */

{OpenBracket}			{ LexLog("OpenBracket"); return (int)Token.OPEN_BRACKET; }
{CloseBracket}			{ LexLog("CloseBracket"); return (int)Token.CLOSE_BRACKET; }
{Comma}					{ LexLog("Comma"); return (int)Token.COMMA; }
{Equal}					{ LexLog("Equal"); return (int)Token.EQUAL; }

{BindingKeyword}		{ LexLog("Binding"); return (int)Token.BINDING; }
{ResourceKeyword}		{ LexLog("Resource"); return (int)Token.RESOURCE; }
{TranslationKeyword}	{ LexLog("Translation"); return (int)Token.TRANSLATION; }

{Identifier}			{ LexLog("Identifier(" + yytext + ")"); GetContent(); return (int)Token.IDENTIFIER; }


{Space}+		/* skip */
{Eol}+			/* skip */

%%