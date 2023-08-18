using PluralityUtilities.AutoHotkeyScripts.Containers;
using PluralityUtilities.AutoHotkeyScripts.Utilities.Interfaces;
using PluralityUtilities.Logging;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class InputParser : IInputParser
	{
		private const string EntriesToken = "entries";
		private const string FieldsToken = "fields";
		private const string TemplatesToken = "templates";
		private IEntryParser EntryParser { get; set; }
		private IFieldParser FieldParser { get; set; }
		private ITemplateParser TemplateParser { get; set; }


		public InputParser( IFieldParser fieldParser, ITemplateParser templateParser, IEntryParser entryParser )
		{
			FieldParser = fieldParser;
			TemplateParser = templateParser;
			EntryParser = entryParser;
		}


		public Input ParseInputFile( string inputFilePath )
		{
			Log.Info( $"started parsing input file: { inputFilePath }");
			var data = ReadDataFromFile( inputFilePath );
			var input = ParseInputData( data );
			Log.Info( "finished parsing input file" );
			return input;
		}


		private Input ParseInputData( string[] data )
		{
			var input = new Input();
			//var tokenParser = new TokenParser();
			//var expectedTokens = new string[]
			//{
			//	EntriesToken,
			//	FieldsToken,
			//	TemplatesToken,
			//};

			//for ( var i = 0; i < data.Length; ++i )
			//{
			//	var rawToken = data[ i ];
			//	var qualifiedToken = tokenParser.ParseToken( rawToken, expectedTokens );
			//	string? errorMessage;
			//	switch ( qualifiedToken.Qualifier )
			//	{
			//		case TokenQualifiers.Recognized:
			//			if ( string.Compare( qualifiedToken.Value, EntriesToken ) == 0 )
			//			{
			//				++i;
			//				input.Entries = EntryParser.ParseEntriesFromData( data, ref i );
			//			}
			//			else if ( string.Compare( qualifiedToken.Value, FieldsToken ) == 0 )
			//			{
			//				++i;
			//				//input.Templates = TemplateParser.ParseTemplatesFromData( data, ref i );
			//			}
			//			else if ( string.Compare( qualifiedToken.Value, TemplatesToken ) == 0 )
			//			{
			//				++i;
			//				input.Templates = TemplateParser.ParseTemplatesFromData( data, ref i );
			//			}
			//			if ( tokenParser.IndentLevel > 0 )
			//			{
			//				throw new RegionNotClosedException( $"input file contains invalid data: a region was not closed properly when parsing token \"{ qualifiedToken.Value }\"" );
			//			}
			//			break;

			//		case TokenQualifiers.BlankLine:
			//			break;

			//		case TokenQualifiers.Unknown:
			//		default:
			//			throw new UnknownTokenException( $"input file contains invalid data: an unknown token ( \"{ qualifiedToken.Value }\" ) was read when a region name was expected" );
			//	}
			//}

			return input;
		}

		private static string[] ReadDataFromFile( string inputFilePath )
		{
			try
			{
				var data = File.ReadAllLines( inputFilePath );
				Log.Info( "successfully read data from input file" );
				return data;
			}
			catch ( Exception e )
			{
				var rethrow = new FileNotFoundException( "failed to read data from input file", e );
				Log.Exception( rethrow );
				throw rethrow;
			}
		}
	}
}
