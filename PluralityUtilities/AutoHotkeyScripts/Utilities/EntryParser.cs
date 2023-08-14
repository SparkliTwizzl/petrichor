using PluralityUtilities.AutoHotkeyScripts.Containers;
using PluralityUtilities.AutoHotkeyScripts.Enums;
using PluralityUtilities.AutoHotkeyScripts.Exceptions;
using PluralityUtilities.AutoHotkeyScripts.Utilities.Interfaces;
using PluralityUtilities.Logging;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class EntryParser : IEntryParser
	{
		private readonly TokenParser TokenParser = new();


		public EntryParser() { }


		public Entry[] ParseEntriesFromData( string[] data, ref int i )
		{
			//Logger.WriteLine( "started parsing entries from input data");
			var entries = new List< Entry >();
			//var expectedTokens = new string[]
			//{
			//	"%",
			//	"$",
			//	"&",
			//};

			//string? exception;
			//for ( ; i < data.Length; ++i )
			//{
			//	var isParsingFinished = false;
			//	var trimmedLine = data[ i ].Trim();
			//	var firstChar = trimmedLine.FirstOrDefault();
			//	var token = TokenParser.ParseToken( firstChar.ToString(), expectedTokens );
			//	switch ( token.Qualifier )
			//	{
			//		case TokenQualifiers.BlankLine:
			//			break;
			//		case TokenQualifiers.CloseBracket:
			//			if ( TokenParser.IndentLevel == 0 )
			//			{
			//				isParsingFinished = true;
			//			}
			//			break;
			//		case TokenQualifiers.Unknown:
			//			throw new UnexpectedCharacterException( $"parsing entries failed at token # { i } :: input file contains invalid data: a line started with a character ( \"{ firstChar }\" ) that was not expected at this time" );
			//		default:
			//			if ( TokenParser.IndentLevel > 1 )
			//			{
			//				++i;
			//				var entry = ParseEntry( data, ref i );
			//				entries.Add( entry );
			//				Logger.Write( "successfully parsed entry: names/tags [" );
			//				foreach ( Identity identity in entry.Identities )
			//				{
			//					Logger.Write( $"{ identity.Name }/{ identity.Tag }, " );
			//				}
			//				Logger.WriteLine( $"], pronoun [{ entry.Pronoun }], decoration [{ entry.Decoration }]" );
			//			}
			//			break;
			//	}
			//	if ( isParsingFinished )
			//	{
			//		break;
			//	}
			//}
			//if ( TokenParser.IndentLevel > 0 )
			//{
			//	throw new InputEntryNotClosedException( "input file contains invalid data: an entry was not closed" );
			//}
			//Logger.WriteLine( "finished parsing entries from input data" );
			return entries.ToArray();
		}


		private static void ParseDecoration( string line, ref Entry entry )
		{
			if ( entry.Decoration != string.Empty )
			{
				throw new DuplicateInputFieldException( "input file contains invalid data: an entry contained more than one decoration field" );
			}
			if ( line.Length < 2 )
			{
				throw new BlankInputFieldException( "input file contains invalid data: an entry contained a blank decoration field" );
			}
			entry.Decoration = line[ 1 .. ];
		}

		private static void ParseIdentity( string line, ref Entry entry )
		{
			Identity identity = new();
			ParseName( line, ref identity );
			ParseTag( line, ref identity );
			entry.Identities.Add( identity );
		}

		private static LineTypes ParseLine( string line, ref Entry entry )
		{
			line = line.TrimStart();
			var firstChar = line[ 0 ];
			switch ( firstChar )
			{
				case '{':
					return LineTypes.EntryStart;
				case '}':
					return LineTypes.EntryEnd;
				case '%':
					ParseIdentity( line, ref entry );
					return LineTypes.Name;
				case '$':
					ParsePronoun( line, ref entry );
					return LineTypes.Pronoun;
				case '&':
					ParseDecoration( line, ref entry );
					return LineTypes.Decoration;
				default:
					return LineTypes.Unknown;
			}
		}

		private static void ParseName( string line, ref Identity identity )
		{
			var fieldStart = line.IndexOf( '#' );
			var fieldEnd = line.LastIndexOf( '#' );
			if ( fieldStart < 0 )
			{
				throw new MissingInputFieldException( "input file contains invalid data: an entry had no name fields" );
			}
			var name = line[ ( fieldStart + 1 ) .. fieldEnd ];
			if ( name.Length < 1 )
			{
				throw new BlankInputFieldException( "input file contains invalid data: an entry contained a blank name field" );
			}
			identity.Name = name;
		}

		private Entry ParseEntry( string[] data, ref int i )
		{
			Logger.WriteLine( "started parsing next entry" );
			var entry = new Entry();
			for ( ; i < data.Length; ++i )
			{
				var line = data[ i ];
				var lineType = ParseLine( line, ref entry );
				switch ( lineType )
				{
					case LineTypes.EntryEnd:
						if ( entry.Identities.Count < 1 )
						{
							throw new MissingInputFieldException( $"parsing entries failed at token # { i } :: input file contains invalid data: an entry did not contain any identity fields" );
						}
						--TokenParser.IndentLevel;
						return entry;
					case LineTypes.Unknown:
						var unexpectedChar = line.Trim()[ 0 ];
						throw new UnexpectedCharacterException( $"parsing entries failed at token # { i } :: input file contains invalid data: a line started with a character ( \"{ unexpectedChar }\" ) that was not expected at this time" );
					default:
						break;
				}
			}
			throw new InputEntryNotClosedException( "input file contains invalid data: last entry was not closed" );
		}

		private static void ParsePronoun( string line, ref Entry entry )
		{
			if ( entry.Pronoun != string.Empty )
			{
				throw new DuplicateInputFieldException( "input file contains invalid data: an entry contained more than one pronoun field" );
			}
			if ( line.Length < 2 )
			{
				throw new BlankInputFieldException( "input file contains invalid data: an entry contained a blank pronoun field" );
			}
			entry.Pronoun = line[ 1 .. ];
		}

		private static void ParseTag( string line, ref Identity identity )
		{
			var fieldStart = line.IndexOf( '@' );
			if ( fieldStart < 0 )
			{
				throw new MissingInputFieldException( "input file contains invalid data: an entry contained an identity field without a tag field" );
			}
			var lastSpace = line.LastIndexOf( ' ' );
			if ( lastSpace >= fieldStart )
			{
				throw new InvalidInputFieldException( "input file contains invalid data: tag fields cannot contain spaces" );
			}
			var tag = line[ ( fieldStart + 1 ) .. ];
			if ( tag.Length < 1 )
			{
				throw new BlankInputFieldException( "input file contains invalid data: an entry contained a blank tag field" );
			}
			identity.Tag = tag;
		}
	}
}
