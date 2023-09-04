namespace PluralityUtilities.Common.Utilities
{
	public static class StringExtensions
	{
		public static string GetDirectory( this string filePath )
		{
			var pathEnd = Math.Max( filePath.LastIndexOf( '/' ), filePath.LastIndexOf( '\\' ) );
			if ( pathEnd < 0 )
			{
				return string.Empty;
			}
			var directoryLength = filePath.Length - ( ( filePath.Length - pathEnd ) - 1 );
			return filePath.Substring( 0, directoryLength );
		}

		public static string GetFileName( this string filePath )
		{
			var pathEnd = Math.Max( filePath.LastIndexOf( '/' ), filePath.LastIndexOf( '\\' ) );
			if ( pathEnd < 0 )
			{
				return filePath;
			}
			var fileNameStart = pathEnd + 1;
			return filePath.Substring( fileNameStart, filePath.Length - fileNameStart );
		}

		public static string RemoveFileExtension( this string filePath )
		{
			var extensionStart = filePath.LastIndexOf( '.' );
			if ( extensionStart < 0 )
			{
				return filePath;
			}
			var filepathLength = filePath.Length - ( filePath.Length - extensionStart );
			return filePath.Substring( 0, filepathLength );
		}

		public static int IndexOfUnescaped( this string source, char toFind, char escapement = '\\' )
		{
			if ( string.IsNullOrEmpty( source ) )
			{
				return -1;
			}

			for ( var i = 0; i < source.Length; ++i )
			{
				if ( source[ i ] == escapement )
				{
					++i; // <- the next character is escaped, skip it
				}
				else if ( source[ i ] == toFind )
				{
					return i;
				}
			}

			return -1;
		}
	}
}