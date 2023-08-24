using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;
using PluralityUtilities.Logging;

namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class EntryDataParser
	{
		public const string RegionName = "entries";
		public string[] AllowedFieldNames { get; set; }
		public Dictionary<string, string[]> FieldDictionary { get; set; }


		public EntryDataParser( Dictionary<string, string[]> fieldDictionary )
		{
			FieldDictionary = fieldDictionary;
			var allowedFieldNameList = new List<string>();
			foreach ( var item in fieldDictionary )
			{
				allowedFieldNameList.Add( item.Key );
			}
			AllowedFieldNames = allowedFieldNameList.ToArray();
		}


		public Token[] ParseEntryDataRegion( Token regionToken )
		{
			var tokenList = TokenDataParser.FlattenTokenTree( regionToken );
			RegionDataValidator.ValidateBasicRegionData( tokenList, RegionName, AllowedFieldNames );
			ValidateFieldStructure( tokenList );
			throw new NotImplementedException();
		}


		private void ValidateFieldStructure( Token[] tokens )
		{
			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				var allowedChildFieldNames = FieldDictionary[ token.Name ];
				foreach ( var childToken in token.Body )
				{
					var isChildNameAnAllowedField = false;

					foreach ( var allowedFieldName in allowedChildFieldNames )
					{
						if ( string.Compare( childToken.Name, allowedFieldName ) == 0 )
						{
							isChildNameAnAllowedField = true;
							break;
						}
					}

					if ( !isChildNameAnAllowedField )
					{
						var e = new InvalidDataException( $"a token with name '{ token.Name }' in region '{ RegionName }' contained a subtoken with a name that did not match field definitions [[ { childToken.Name } ]]" );
						Log.Exception( e );
						throw e;
					}
				}
			}
		}
	}
}
