﻿using Petrichor.Common.Containers;
using Petrichor.Common.Exceptions;
using Petrichor.Logging;

namespace Petrichor.Common.Utilities
{
	public class RegionParser< T > : IRegionParser< T > where T : class, new()
	{
		private int IndentLevel { get; set; } = 0;
		private Func< T, T > PostParseHandler { get; set; } = ( T result ) => new T();
		private Func< T > PreParseHandler { get; set; } = () => new T();
		private Dictionary< string, Func< string[], int, T, RegionData< T > > > TokenHandlers { get; set; } = new();


		public int LinesParsed { get; private set; } = 0;
		public Dictionary< string, int > MaxAllowedTokenInstances { get; private set; } = new();
		public Dictionary< string, int > MinRequiredTokenInstances { get; private set; } = new();
		public string RegionName { get; private set; } = string.Empty;
		public Dictionary< string, int > TokenInstancesParsed { get; private set; } = new();


		public RegionParser() { }

		public RegionParser( RegionParserDescriptor< T > descriptor )
		{
			MaxAllowedTokenInstances = descriptor.MaxAllowedTokenInstances;
			MinRequiredTokenInstances = descriptor.MinRequiredTokenInstances;
			PostParseHandler = descriptor.PostParseHandler;
			PreParseHandler = descriptor.PreParseHandler;
			RegionName = descriptor.RegionName;
			TokenHandlers = descriptor.TokenHandlers;
		}

		public T Parse( string[] regionData )
		{
			var taskMessage = $"Parse \"{ RegionName }\" region";
			Log.TaskStart( taskMessage );

			foreach ( var tokenName in MinRequiredTokenInstances.Keys )
			{
				TokenInstancesParsed.Add( tokenName, 0 );
			}

			if ( !TokenHandlers.TryGetValue( RegionName, out var value ) )
			{
				TokenHandlers.Add( RegionName, ( string[] regionData, int tokenStartIndex, T result ) => new() );
			}

			var result = PreParseHandler();

			for ( var i = 0 ; i < regionData.Length ; ++i )
			{
				var rawToken = regionData[ i ];
				var token = new StringToken( rawToken );

				if ( token.Name == string.Empty )
				{
					continue;
				}

				else if ( token.Name == Syntax.Tokens.RegionOpen )
				{
					++IndentLevel;
					continue;
				}

				else if ( token.Name == Syntax.Tokens.RegionClose )
				{
					--IndentLevel;

					if ( IndentLevel < 0 )
					{
						ExceptionLogger.LogAndThrow( new BracketException( $"A mismatched closing bracket was found in a \"{ RegionName }\" region." ) );
					}

					if ( IndentLevel == 0 )
					{
						LinesParsed = i + 1;
						break;
					}

					continue;
				}

				else if ( TokenHandlers.TryGetValue( token.Name, out var handler ) )
				{
					if ( token.Value == string.Empty )
					{
						StringToken? nextToken = i < regionData.Length - 1 ? new( regionData[ i + 1 ] ) : null;
						if ( nextToken is not null && nextToken.Name != Syntax.TokenNames.RegionOpen )
						{
							Log.Warning( $"Parsed a \"{ token.Name }\" token with no value." );
						}
					}

					++TokenInstancesParsed[ token.Name ];
					var handlerResult = handler( regionData, i, result );
					i += handlerResult.BodySize;
					result = handlerResult.Value;
					continue;
				}

				// this can only be reached if a token is not recognized and therefore not handled
				ExceptionLogger.LogAndThrow( new TokenException( $"An unrecognized token (\"{ rawToken.Trim() }\") was found in a \"{ RegionName }\" region." ) );
			}

			if ( IndentLevel != 0 )
			{
				ExceptionLogger.LogAndThrow( new BracketException( $"A mismatched curly brace was found in a \"{ RegionName }\" region." ) );
			}

			foreach ( var tokenName in TokenInstancesParsed.Keys )
			{
				var instances = TokenInstancesParsed[ tokenName ];
				var minRequiredInstances = MinRequiredTokenInstances[ tokenName ];
				var maxAllowedInstances = MaxAllowedTokenInstances[ tokenName ];

				var hasTooManyInstances = instances > maxAllowedInstances;
				var hasTooFewInstances = instances < minRequiredInstances;

				if ( hasTooFewInstances || hasTooManyInstances )
				{
					ExceptionLogger.LogAndThrow( new TokenException( $"\"{ RegionName }\" regions must contain at least { minRequiredInstances } and no more than { maxAllowedInstances } \"{ tokenName }\" tokens." ) );
				}
			}

			result = PostParseHandler( result );

			Log.TaskFinish( taskMessage );
			return result;
		}
	}
}
