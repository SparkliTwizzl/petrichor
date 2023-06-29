using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Enums;
using PluralityUtilities.Common.Exceptions;
using PluralityUtilities.Logging;
using System.Text.RegularExpressions;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class TokenParser
	{
		public int IndentLevel { get; set; } = 0;

		public TokenParser() { }


		public QualifiedToken ParseToken( string token, string[] expectedValues )
		{
			Log.WriteLineTimestamped( $"started parsing token \"{ token }\", expecting a value from:");
			foreach ( var tokenValue in expectedValues )
			{
				Log.WriteLineTimestamped( $"	{ tokenValue }" );
			}

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

			return qualifiedToken;
		}

		public Token ParseToken( string[] input )
		{
			Log.WriteLineTimestamped( "attempting to parse a token" );

			var rawLine = input[ 0 ];
			var firstTag = ParseTag( rawLine );

			switch ( firstTag.Type )
			{
				case TagTypes.Open:
					var line = string.Empty;
					var nextTag = Tag.Empty;
					for ( var i = 1; i < input.Length; ++i )
					{
						line = input[ i ];
						nextTag = ParseTag( line );
						if ( nextTag.Type == TagTypes.Close )
						{
						}
					}
					break;
				case TagTypes.Close:
					var message = "a closing tag was read with no corresponding opening tag";
					Log.WriteLineTimestamped( $"error: { message }" );
					throw new InvalidInputException( message );
				case TagTypes.SelfClose:

					break;
				case TagTypes.Invalid:
				default:
					return Token.Empty;
			}
		}


		private Tag ParseTag( string input )
		{
			var tag = new Tag();
			input = input.Trim();

			tag.Type = IdentifyTagType( input );
			var tagStartString = Tag.GetTagStartString ( tag.Type );
			var tagEndString = Tag.GetTagEndString ( tag.Type );
			var valueStart = input.IndexOf( tagStartString );
			var valueEnd = input.IndexOf( tagEndString );
			tag.Value = input[ valueStart .. valueEnd ];

			return tag;
		}

		private TagTypes IdentifyTagType( string input )
		{
			if ( Regex.IsMatch( input, Tag.OpenTagRegexMatch ) )
			{
				return TagTypes.Open;
			}
			if ( Regex.IsMatch( input, Tag.CloseTagRegexMatch ) )
			{
				return TagTypes.Close;
			}
			if ( Regex.IsMatch( input, Tag.SelfCloseTagRegexMatch ) )
			{
				return TagTypes.SelfClose;
			}
			else
			{
				return TagTypes.Invalid;
			}
		}
	}
}
