using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;
using PluralityUtilities.Logging;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class TemplateParser
	{
		private const string RegionName = "templates";
		private static string[] ValidTokenNames { get; } = new string[]
		{
			"template",
			"template-manual",
		};


		public static string[] ParseTemplateData( Token regionToken )
		{
			var tokenList = TokenParser.FlattenTokenTree( regionToken );
			RegionDataValidator.ValidateBasicRegionData( tokenList, RegionName, ValidTokenNames );
			ValidateTemplateStrings( tokenList );
			var result = BuildTemplateList( tokenList );
			return result;
		}


		private static string[] BuildTemplateList( Token[] tokens )
		{
			throw new NotImplementedException();
		}

		private static void ValidateTemplateStrings( Token[] tokens )
		{
			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				switch ( token.Name )
				{
					case "template":
						ValidateStandardTemplateString(token.Value);
						break;

					case "template-manual":
						ValidateManualTemplateString( token.Value );
						break;

					default:
						var e = new InvalidDataException( $"token was found with unrecognized name while parsing templates. THIS SHOULD NOT BE POSSIBLE, REPORT THIS! [[ '{ token.Name }' ]]" );
						Log.Exception( e );
						throw e;
				}
			}
		}

		private static void ValidateManualTemplateString( string rawTemplate )
		{
			throw new NotImplementedException();
			//TODO ensure leading divider is present
			//TODO ensure trailing divider is present
			//TODO ensure replace string is present
		}

		private static void ValidateStandardTemplateString( string rawTemplate )
		{
			throw new NotImplementedException();
			//TODO ensure prefix char is present
			//TODO ensure replace field is present
			//TODO ensure divider is present
		}
	}
}
