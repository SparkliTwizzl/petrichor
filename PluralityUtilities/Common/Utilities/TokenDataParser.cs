using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using System.Text.RegularExpressions;


namespace PluralityUtilities.Common.Utilities
{
	public static partial class TokenDataParser
	{
		public static Token[] FlattenTokenTree( Token token)
		{
			var result = new List<Token>()
			{
				token
			};

			for ( var i = 0; i < result.Count; ++i )
			{
				var parent = result[ i ];
				foreach ( var child in parent.Body )
				{
					result.Add( child );
				}
			}

			return result.ToArray();
		}

		public static Token ParseTokenFromString( string input )
		{
			var line = input.Trim();
			var token = new Token
			{
				Name = GetTokenNameFromString( line ),
				Value = GetTokenValueFromString( line ),
			};
			return token;
		}


		// allowed characters: [[ 'A'-'Z' 'a'-'z' '0'-'9' '-' ]]; if anything not in this set is found, token name is invalid
		[ GeneratedRegex( "([^A-Za-z0-9-])+" ) ]
		private static partial Regex AllowedTokenNameRegex();

		private static string GetTokenNameFromString( string text )
		{
			var substrings = text.Split( ':' );
			if ( substrings.Length < 2 || text.IndexOf( ':' ) < 1 )
			{
				throw new InvalidTokenException( $"invalid token [[ { text } ]], tokens must have a name and a value separated by a colon ( : )" );
			}
			var tokenName = substrings[ 0 ].Trim();
			if ( AllowedTokenNameRegex().Match( tokenName ) != Match.Empty )
			{
				throw new InvalidTokenException( $"invalid token name [[ { tokenName } ]], token names can only contain alphanumeric characters ( A-Z, a-z, 0-9 ) and dashes ( - ) ");
			}
			return tokenName;
		}

		private static string GetTokenValueFromString( string text )
		{
			var tokenValueStartsAtIndex = text.IndexOf( ':' ) + 1;
			if ( tokenValueStartsAtIndex < 0 )
			{
				throw new InvalidTokenException( $"invalid token [[ { text } ]], tokens must have a name and a value separated by a colon ( : )" );
			}
			var tokenValue = text[ tokenValueStartsAtIndex .. ].Trim();
			return tokenValue;
		}
	}
}
