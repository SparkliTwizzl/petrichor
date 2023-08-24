using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;
using PluralityUtilities.Logging;
using System.Text.RegularExpressions;

namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public partial class TemplateParser
	{
		private const string RegionName = "templates";
		private const string BasicTemplateTokenName = "template";
		private const string ManualTemplateTokenName = "template-manual";
		private static string[] ValidTokenNames { get; } = new string[]
		{
			"template",
			"template-manual",
		};


		public static string[] ParseTemplateData( Token regionToken )
		{
			var tokenList = TokenParser.FlattenTokenTree( regionToken );
			RegionDataValidator.ValidateBasicRegionData( tokenList, RegionName, ValidTokenNames );
			RegionDataValidator.RejectNestedTokens( tokenList, RegionName );
			ValidateTemplateStrings( tokenList );
			var result = BuildTemplateList( tokenList );
			return result;
		}


		private static string[] BuildTemplateList( Token[] tokens )
		{
			var result = new List<string>();
			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				switch( token.Name )
				{
					case BasicTemplateTokenName:
						var templateString = ParseBasicTemplateString( token.Value );
						result.Add( templateString );
						break;

					case ManualTemplateTokenName:
						result.Add( token.Value );
						break;

					default:
						var e = new InvalidDataException( $"a token was found with an unrecognized name while parsing templates. THIS SHOULD NOT BE POSSIBLE, REPORT THIS! [[ '{ token.Name }' ]]" );
						Log.Exception( e );
						throw e;
				}
			}
			return result.ToArray();
		}

		private static string ParseBasicTemplateString( string templateString )
		{
			var replaceStringEnd = templateString.IndexOf( '}' ) + 1;
			var replaceString = templateString[ 0 .. replaceStringEnd ];
			var replacementStringStart = templateString.IndexOf( ':' ) + 1;
			var replacementString = templateString[ replacementStringStart .. ];
			return $"::{ replaceString }::{ replacementString }";
		}

		private static void ValidateTemplateStrings( Token[] tokens )
		{
			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				switch ( token.Name )
				{
					case ManualTemplateTokenName:
						ValidateManualTemplateString( token.Value );
						break;

					case BasicTemplateTokenName:
						ValidateBasicTemplateString( token.Value );
						break;

					default:
						var e = new InvalidDataException( $"a token was found with an unrecognized name while parsing templates. THIS SHOULD NOT BE POSSIBLE, REPORT THIS! [[ '{ token.Name }' ]]" );
						Log.Exception( e );
						throw e;
				}
			}
		}

		private static void ValidateBasicTemplateString( string rawTemplate )
		{
			if ( ValidBasicTemplateRegex().Match( rawTemplate ) == Match.Empty )
			{
				var e = new InvalidDataException( $"a token with name '{ BasicTemplateTokenName }' had a value that was not a valid standard template [[ '{ rawTemplate }' ]]" );
				Log.Exception( e );
				throw e;
			}
		}

		private static void ValidateManualTemplateString( string rawTemplate )
		{
			if ( ValidManualTemplateRegex().Match( rawTemplate ) == Match.Empty )
			{
				var e = new InvalidDataException( $"a token with name '{ ManualTemplateTokenName }' had a value that was not a valid manual template [[ '{ rawTemplate }' ]]" );
				Log.Exception( e );
				throw e;
			}
		}

		// standard templates must have a single prefix character ( . ), a replace field with a name
		//   at least one character long ( {.+} ), and a divider ( : ); a replacement string is
		//   optional ( .* ).
		[ GeneratedRegex( ".{.+}:.*" ) ]
		private static partial Regex ValidBasicTemplateRegex();

		// manual templates must have a leading divider ( :: ), a replace string with at least one
		//   character ( .+ ), and a trailing divider ( :: ); a replacement string is
		//   optional ( .* ).
		[ GeneratedRegex( "::.+::.*" ) ]
		private static partial Regex ValidManualTemplateRegex();
	}
}
