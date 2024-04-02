from pygments.lexer import *
from pygments.token import *


class PetrichorScriptLexer(RegexLexer):
	name = 'PetrichorScript'
	aliases = ['petrichor']
	filenames = ['*.petrichor','*.ptcr']

	tokens = {
		'root': [
			(r'\s+', Text), # whitespace
			(r'\\.', String.Escape), # escaped character
			(r'//.*\n', Comment.Single),
			(r'[\{\},]', Operator),
			(r'::', Operator),
			(r'([a-z\-]+)(\s*)(:(?!:))', bygroups(Keyword.Reserved, Text, Operator)),
			(r'\[[a-z\-]+\]', Name.Variable),
			(r'".*"', String),
			(r'[0-9]+', Number),
			(r'.', Text),
		]
	}
