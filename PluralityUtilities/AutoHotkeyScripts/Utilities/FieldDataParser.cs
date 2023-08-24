using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public static class FieldDataParser
	{
		private const string RegionName = "fields";
		private static string[] ValidTokenNames { get; } = new string[]
		{
			"field",
		};


		public static Dictionary<string, string[]> ParseRegionData( Token regionToken )
		{
			var tokenList = TokenDataParser.FlattenTokenTree( regionToken );
			RegionDataValidator.ValidateBasicRegionData( tokenList, RegionName, ValidTokenNames );
			RegionDataValidator.RejectDuplicateTokenValues( tokenList, RegionName );
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
	}
}
