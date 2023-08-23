using PluralityUtilities.AutoHotkeyScripts.Exceptions;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Logging;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public static class RegionDataValidator
	{
		public static void RejectDuplicateTokenValues( Token[] tokens, string regionName )
		{
			var values = new List<string>();
			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				if ( values.Contains( token.Value ) )
				{
					var e = new DuplicateValueException( $"region '{ regionName }' contained a token with a duplicate value, but values in this region must be unique [[ { token.Value } ]]" );
					Log.Exception( e );
					throw e;
				}
				values.Add( token.Value );
			}
		}

		public static void ValidateBasicRegionData( Token[] tokens, string regionName, string[] validTokenNames )
		{
			var shouldBeRegion = tokens[ 0 ];
			if ( shouldBeRegion.Name != "region" || shouldBeRegion.Value != regionName )
			{
				var e = new MissingValueException( $"input file must contain a region token with the value '{ regionName }' (ie, 'region:{ regionName }') to contain definition tokens" );
				Log.Exception( e );
				throw e;
			}

			if ( tokens.Length < 2 )
			{
				var e = new MissingValueException( $"region '{ regionName }' cannot be empty; it must have child tokens containing definitions" );
				Log.Exception( e );
				throw e;
			}

			for ( var i = 1; i < tokens.Length; ++i )
			{
				var token = tokens[ i ];
				var isTokenNameValid = false;
				foreach ( var validName in validTokenNames )
				{
					if ( token.Name == validName )
					{
						isTokenNameValid = true;
						break;
					}
				}
				if ( !isTokenNameValid )
				{
					var e = new InvalidNameException($"region '{ regionName }' contained a token with an invalid name [[ '{ token.Name }' ]]");
					Log.Exception(e);
					throw e;
				}
			}
		}
	}
}
