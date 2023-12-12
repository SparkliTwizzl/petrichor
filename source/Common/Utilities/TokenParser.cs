﻿using Petrichor.Common.Containers;
using Petrichor.Common.Enums;
using Petrichor.Logging;


namespace Petrichor.AutoHotkeyScripts.Utilities
{
	public class TokenParser
	{
		public int IndentLevel { get; set; } = 0;

		public TokenParser() { }


		public QualifiedToken ParseToken( string token, string[] expectedValues )
		{
			var taskMessage = $"parsing token \"{ token }\"";
			Log.TaskStarted( taskMessage );

			var qualifiedToken = new QualifiedToken( token.Trim() );
			if ( string.Compare( qualifiedToken.Value, "" ) == 0 )
			{
				qualifiedToken.Qualifier = TokenQualifiers.BlankLine;
			}
			else if ( string.Compare( qualifiedToken.Value, "{" ) == 0 )
			{
				++IndentLevel;
				qualifiedToken.Qualifier = TokenQualifiers.OpenBracket;
			}
			else if ( string.Compare( qualifiedToken.Value, "}" ) == 0 )
			{
				--IndentLevel;
				qualifiedToken.Qualifier = TokenQualifiers.CloseBracket;
			}
			else
			{
				foreach ( var value in expectedValues )
				{
					if ( string.Compare( qualifiedToken.Value, value ) == 0 )
					{
						qualifiedToken.Qualifier = TokenQualifiers.Recognized;
						break;
					}
				}
			}
			Log.TaskFinished( taskMessage );
			return qualifiedToken;
		}
	}
}
