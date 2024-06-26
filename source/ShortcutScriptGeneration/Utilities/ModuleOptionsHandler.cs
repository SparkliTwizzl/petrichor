﻿using Petrichor.Common.Containers;
using Petrichor.Common.Utilities;
using Petrichor.Logging;
using Petrichor.ShortcutScriptGeneration.Containers;
using Petrichor.ShortcutScriptGeneration.Syntax;


namespace Petrichor.ShortcutScriptGeneration.Utilities
{
	public static class ModuleOptionsHandler
	{
		public static ProcessedRegionData<ModuleOptionData> DefaultIconTokenHandler( IndexedString[] regionData, int tokenStartIndex, ModuleOptionData result )
		{
			var token = new StringToken( regionData[ tokenStartIndex ] );
			var filePath = token.Value.WrapInQuotes();
			result.DefaultIconFilePath = filePath;
			Log.Info( $"Stored default icon file path ({filePath})." );
			return new ProcessedRegionData<ModuleOptionData>( result );
		}

		public static ProcessedRegionData<ModuleOptionData> ReloadShortcutTokenHandler( IndexedString[] regionData, int tokenStartIndex, ModuleOptionData result )
		{
			var token = new StringToken( regionData[ tokenStartIndex ] );
			var hotstring = ReplaceFieldsInScriptControlHotstring( token.Value );
			result.ReloadShortcut = hotstring;
			Log.Info( $"Stored reload shortcut (\"{token.Value}\" -> \"{hotstring}\")." );
			return new ProcessedRegionData<ModuleOptionData>( result );
		}

		public static ProcessedRegionData<ModuleOptionData> SuspendIconTokenHandler( IndexedString[] regionData, int tokenStartIndex, ModuleOptionData result )
		{
			var token = new StringToken( regionData[ tokenStartIndex ] );
			var filePath = token.Value.WrapInQuotes();
			result.SuspendIconFilePath = filePath;
			Log.Info( $"Stored suspend icon file path ({filePath})." );
			return new ProcessedRegionData<ModuleOptionData>( result );
		}

		public static ProcessedRegionData<ModuleOptionData> SuspendShortcutTokenHandler( IndexedString[] regionData, int tokenStartIndex, ModuleOptionData result )
		{
			var token = new StringToken( regionData[ tokenStartIndex ] );
			var hotstring = ReplaceFieldsInScriptControlHotstring( token.Value );
			result.SuspendShortcut = hotstring;
			Log.Info( $"Stored suspend shortcut (\"{token.Value}\" -> \"{hotstring}\")." );
			return new ProcessedRegionData<ModuleOptionData>( result );
		}


		private static string ReplaceFieldsInScriptControlHotstring( string hotstring )
		{
			foreach ( var findTag in ControlShortcutFindAndReplace.LookUpTable )
			{
				var find = findTag.Key;
				var replace = findTag.Value;
				hotstring = hotstring.Replace( find, replace );
			}

			return hotstring
				.Replace( $"{Common.Syntax.ControlSequences.Escape}{Common.Syntax.ControlSequences.FindTagOpen}", Common.Syntax.ControlSequences.FindTagOpen.ToString() )
				.Replace( $"{Common.Syntax.ControlSequences.Escape}{Common.Syntax.ControlSequences.FindTagClose}", Common.Syntax.ControlSequences.FindTagClose.ToString() );
		}
	}
}
