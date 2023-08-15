using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using System.Text.RegularExpressions;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public partial class TokenParser
	{
		public TokenParser() { }


		public static Token ParseTokenFromString( string text )
		{
			var trimmedText = text.Trim();
			var token = new Token
			{
				Name = GetTokenNameFromString( trimmedText ),
				Value = GetTokenValueFromString( trimmedText ),
			};
			return token;
		}


		[ GeneratedRegex( "([^A-Za-z0-9-])+" ) ]
		private static partial Regex AllowedTokenNameRegex();

		private static string GetTokenNameFromString( string text )
		{
			var substrings = text.Split( ':' );
			if ( substrings.Length < 2 )
			{
				throw new InvalidTokenException( $"invalid token [[ { text } ]], tokens must have a name and a value separated by a colon ( : )" );
			}
			var tokenName = substrings[ 0 ].Trim();
			// allowed characters: [[ 'A'-'Z' 'a'-'z' '0'-'9' '-' ]]; if anything not in this set is found, token name is invalid
			if ( AllowedTokenNameRegex().Match( tokenName ) != Match.Empty )
			{
				throw new InvalidTokenException( $"invalid token name [[ { tokenName } ]], token names can only contain alphanumeric characters ( A-Z, a-z, 0-9 ) and dashes ( - ) ");
			}
			return tokenName;
		}

		private static string GetTokenValueFromString( string text )
		{
			var tokenValueStartsAtIndex = text.IndexOf( ':' );
			if ( tokenValueStartsAtIndex < 0 )
			{
				throw new InvalidTokenException( $"invalid token [[ { text } ]], tokens must have a name and a value separated by a colon ( : )" );
			}
			var tokenValue = text[ tokenValueStartsAtIndex .. ].Trim();
			return tokenValue;
		}
	}
}
