using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using System.Text.RegularExpressions;

namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class TokenParser
	{
		public int IndentLevel { get; set; } = 0;


		public TokenParser() { }


		//TODO write test case for valid data
		//TODO write test case for invalid name
		//TODO write test case for missing name
		//TODO write test case for missing value
		public Token ParseTokenFromString( string text )
		{
			var token = new Token();
			var trimmedText = text.Trim();
			token.Name = GetTokenNameFromString( trimmedText );
			token.Value = GetTokenValueFromString( trimmedText );
			return token;
		}


		private static string GetTokenNameFromString( string text )
		{
			var substrings = text.Split( ':' );
			if ( substrings.Length < 2 )
			{
				throw new InvalidTokenException( $"invalid token [[ { text } ]], tokens must have a name and a value separated by a colon ( : )" );
			}
			var tokenName = substrings[ 0 ].Trim();
			// allowed characters: [[ 'A'-'Z' 'a'-'z' '0'-'9' '-' ]]; if anything not in this set is found, token name is invalid
			if ( Regex.Match( tokenName, "([^A-Za-z0-9-])+") != Match.Empty )
			{
				throw new InvalidTokenException( $"invalid token name [[ { tokenName } ]], token names can only contain alphanumeric characters ( A-Z, a-z, 0-9 ) and dashes ( - ) ");
			}
			return tokenName;
		}

		private string GetTokenValueFromString( string text )
		{
			//TODO split text at :
			//TODO discard first half
			//TODO return remainder
			return text;
		}
	}
}
