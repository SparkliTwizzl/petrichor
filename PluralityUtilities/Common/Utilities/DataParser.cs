using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using PluralityUtilities.Logging;

namespace PluralityUtilities.Common.Utilities
{
	public class DataParser
	{
		public int Indent { get; set; } = 0;


		private struct RecurseParseRawDataResult
		{
			public Token TokenToPopulate { get; set; }
			public int NumberOfLinesParsed { get; set; }
		}


		public DataParser() { }


		public Token ParseRawData( string[] rawData )
		{
			return RecurseParseRawData( rawData, Token.Empty ).TokenToPopulate;
		}


		private RecurseParseRawDataResult RecurseParseRawData( string[] rawData, Token parent )
		{
			var result = new RecurseParseRawDataResult
			{
				TokenToPopulate = parent,
			};

			for ( var i = 0; i < rawData.Length; ++i )
			{
				var line = rawData[ i ].Trim();
				switch ( line )
				{
					case "{":
						++Indent;
						// recurse to populate body of most recent token
						var recurseResult = RecurseParseRawData( rawData[ (i + 1) .. ], result.TokenToPopulate.Body.Last() );
						result.TokenToPopulate.Body.Last().Body = recurseResult.TokenToPopulate.Body;
						i += recurseResult.NumberOfLinesParsed;
						continue;

					case "}":
						--Indent;
						// skip lines parsed by recursive call
						result.NumberOfLinesParsed = i + 1;
						return result;

					default:
						result.TokenToPopulate.Body.Add( TokenParser.ParseTokenFromString( line ) );
						break;
				}
			}

			if ( Indent != 0 )
			{
				var e = new IndentImbalanceException( "open and close brackets ( '{' '}' ) must be balanced" );
				Log.Exception( e );
				throw e;
			}

			return result;
		}
	}
}
