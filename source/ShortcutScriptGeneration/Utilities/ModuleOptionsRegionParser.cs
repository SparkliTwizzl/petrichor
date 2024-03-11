﻿using Petrichor.Common.Containers;
using Petrichor.Common.Exceptions;
using Petrichor.Common.Utilities;
using Petrichor.Logging;
using Petrichor.ShortcutScriptGeneration.Containers;
using Petrichor.ShortcutScriptGeneration.LookUpTables;


namespace Petrichor.ShortcutScriptGeneration.Utilities
{
	public class ModuleOptionsRegionParser : IModuleOptionsRegionParser
	{
		private int IndentLevel { get; set; } = 0;
		private static string RegionName => Syntax.Tokens.ModuleOptionsRegion;


		public bool HasParsedMaxAllowedRegions { get; private set; } = false;
		public int LinesParsed { get; private set; } = 0;
		public int MaxRegionsAllowed => 1;
		public int RegionsParsed { get; private set; } = 0;


		public ScriptModuleOptions Parse( string[] regionData )
		{
			var taskMessage = $"Parse region: {RegionName}";
			Log.TaskStart( taskMessage );

			if ( HasParsedMaxAllowedRegions )
			{
				ExceptionLogger.LogAndThrow( new FileRegionException( $"Input file cannot contain more than {MaxRegionsAllowed} {RegionName} regions" ) );
			}

			var moduleOptions = new ScriptModuleOptions();
			for ( var i = 0 ; i < regionData.Length ; ++i )
			{
				var rawToken = regionData[ i ];
				var token = new StringToken( rawToken );
				var isParsingFinished = false;

				if ( token.Name == string.Empty )
				{
					continue;
				}

				else if ( token.Name == Common.Syntax.Tokens.RegionOpen )
				{
					++IndentLevel;
				}

				else if ( token.Name == Common.Syntax.Tokens.RegionClose )
				{
					--IndentLevel;

					if ( IndentLevel < 0 )
					{
						ExceptionLogger.LogAndThrow( new BracketException( $"A mismatched closing bracket was found when parsing region: {RegionName}" ) );
					}

					if ( IndentLevel == 0 )
					{
						isParsingFinished = true;
					}
				}

				else if ( token.Name == Syntax.TokenNames.DefaultIconFilePath )
				{
					moduleOptions.DefaultIconFilePath = token.Value.WrapInQuotes();
					Log.Info( $"Stored token { Syntax.TokenNames.DefaultIconFilePath }" );
				}

				else if ( token.Name == Syntax.TokenNames.ReloadShortcut )
				{
					moduleOptions.ReloadShortcut = ReplaceFieldsInShortcut( token.Value );
					Log.Info( $"Stored token { Syntax.TokenNames.ReloadShortcut }" );
				}

				else if ( token.Name == Syntax.TokenNames.SuspendIconFilePath )
				{
					moduleOptions.SuspendIconFilePath = token.Value.WrapInQuotes();
					Log.Info( $"Stored token { Syntax.TokenNames.SuspendIconFilePath }" );
				}

				else if ( token.Name == Syntax.TokenNames.SuspendShortcut )
				{
					moduleOptions.SuspendShortcut = ReplaceFieldsInShortcut( token.Value );
					Log.Info( $"Stored token { Syntax.TokenNames.SuspendShortcut }" );
				}

				else
				{
					ExceptionLogger.LogAndThrow( new TokenException( $"An unrecognized token (\"{ rawToken.Trim() }\") was found when parsing region: { RegionName }" ) );
				}

				if ( isParsingFinished )
				{
					LinesParsed = i + 1;
					break;
				}
			}

			if ( IndentLevel != 0 )
			{
				ExceptionLogger.LogAndThrow( new BracketException( $"A mismatched curly brace was found when parsing region: {RegionName}" ) );
			}

			++RegionsParsed;
			HasParsedMaxAllowedRegions = RegionsParsed >= MaxRegionsAllowed;

			Log.TaskFinish( taskMessage );
			return moduleOptions;
		}

		private static string ReplaceFieldsInShortcut( string shortcut )
		{
			foreach ( var keyValuePair in ScriptHotstringKeys.LookUpTable )
			{
				var find = keyValuePair.Key;
				var replace = keyValuePair.Value;
				shortcut = shortcut.Replace( find, replace );
			}

			shortcut = shortcut.Replace( $"{ Common.Syntax.OperatorChars.Escape }[", "[" )
				.Replace( $"{ Common.Syntax.OperatorChars.Escape }]", "]" );

			return shortcut;
		}
	}
}
