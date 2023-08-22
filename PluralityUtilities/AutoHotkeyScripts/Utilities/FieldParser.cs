using PluralityUtilities.AutoHotkeyScripts.Exceptions;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;
using PluralityUtilities.Logging;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class FieldParser
	{
		public static Dictionary<string, string[]> ParseFieldData( Token regionToken )
		{
			var tokenList = TokenParser.FlattenTokenTree( regionToken );
			ValidateInputData( tokenList );
			var result = BuildFieldDictionary( tokenList );
			return result;
		}


		private static Dictionary<string, string[]> BuildFieldDictionary( Token[] tokens )
		{
			var result = new Dictionary<string, string[]>();

			foreach ( var parent in tokens )
			{
				var children = new List<string>();
				foreach ( var child in parent.Body )
				{
					children.Add( child.Value );
				}
				result.Add( parent.Value, children.ToArray() );
			}

			return result;
		}

		private static void ValidateInputData( Token[] tokens )
		{
			var shouldBeRegion = tokens[ 0 ];
			if ( shouldBeRegion.Name != "region" || shouldBeRegion.Value != "fields" )
			{
				var e = new MissingRegionException( "input file must contain a fields region (ie, 'region:fields') to contain field definitions" );
				Log.Exception( e );
				throw e;
			}

			var values = new List<string>();
			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				if ( token.Name != "field" )
				{
					var e = new InvalidNameException( $"'fields' region contained a parent with an invalid name [[ { token.Name } ]]" );
					Log.Exception( e );
					throw e;
				}

				if ( values.Contains( token.Value ) )
				{
					var e = new DuplicateValueException( $"'fields' region contained a parent with a duplicate name [[ { token.Value } ]]" );
					Log.Exception( e );
					throw e;
				}

				values.Add( token.Value );
			}
		}
	}
}
