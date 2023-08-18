using PluralityUtilities.AutoHotkeyScripts.Utilities;
using PluralityUtilities.Common;
using PluralityUtilities.Logging;
using PluralityUtilities.Logging.Enums;


namespace PluralityUtilities.App
{
	static class Program
	{
		private static string InputFilePath { get; set; } = string.Empty;
		private static LoggingMode LogMode { get; set; } = LoggingMode.Disabled;
		private static string OutputFilePath { get; set; } = string.Empty;
		private static DateTime StartTime { get; set; }


		static void Main( string[] args )
		{
			StartTime = DateTime.Now;
			Console.WriteLine( $"PluralityUtilities v{ AppVersion.CurrentVersion }" );
			if ( args.Length < 1 )
			{
				Console.WriteLine( "usage:" );
				Console.WriteLine( "pass input file as arg0" );
				Console.WriteLine( "pass output file as arg1" );
				Console.WriteLine( "pass \"-l\" as arg2 to enable basic logging ( log file output only )" );
				Console.WriteLine( "pass \"-v\" as arg2 to enable verbose logging ( console and log file output )" );
				WaitForUserToExit();
				return;
			}
			ParseArgs( args );
			InitLogging();
			Log.Info( $"PluralityUtilities v{ AppVersion.CurrentVersion }; execution started at { StartTime }" );
			CreateAutoHotkeyScript();
			Log.Info( $"execution finished in { ( DateTime.Now - StartTime ).TotalSeconds } seconds" );
			WaitForUserToExit();
		}


		private static void CreateAutoHotkeyScript()
		{
			try
			{
				var fieldParser = new FieldParser();
				var templateParser = new TemplateParser();
				var entryParser = new EntryParser();
				var inputParser = new InputParser( fieldParser, templateParser, entryParser );
				var scriptGenerator = new AutoHotkeyScriptGenerator( inputParser );

				scriptGenerator.GenerateScriptFromInputFile( InputFilePath, OutputFilePath );
				var successMessage = "generating script succeeded";
				if ( !Log.IsLoggingToConsoleEnabled )
				{
					Console.WriteLine(successMessage);
				}
				Log.Info( successMessage );
			}
			catch ( Exception e )
			{
				if ( !Log.IsLoggingToConsoleEnabled )
				{
					Console.WriteLine( $"generating script failed with error: { e.Message }" );
				}
				Log.Exception( e );
			}
		}

		private static void InitLogging()
		{
			string loggingEnabledMessage;
			switch ( LogMode )
			{
				case LoggingMode.All:
					Log.EnableForAll();
					Log.SetLogFolder( ProjectDirectories.LogDir );
					loggingEnabledMessage = "logging to console and file is enabled";
					break;
				case LoggingMode.ConsoleOnly:
					Log.EnableForConsoleOnly();
					Log.SetLogFolder( ProjectDirectories.LogDir );
					loggingEnabledMessage = "logging to console is enabled";
					break;
				case LoggingMode.FileOnly:
					Log.EnableForFileOnly();
					loggingEnabledMessage = "logging to file is enabled";
					break;
				default:
					loggingEnabledMessage = "logging is disabled";
					break;
			}
			if ( Log.IsLoggingEnabled )
			{
				Log.SetLogFolder( ProjectDirectories.LogDir );
			}

			Console.WriteLine( loggingEnabledMessage );
			Console.WriteLine();
		}

		private static void ParseArgs( string[] args )
		{
			InputFilePath = args[ 0 ];
			OutputFilePath = args[ 1 ];
			if ( args.Length > 2 )
			{
				var arg = args[ 2 ];
				if ( string.Compare( arg, "-a" ) == 0 )
				{
					LogMode = LoggingMode.All;
				}
				else if ( string.Compare( arg, "-c" ) == 0 )
				{
					LogMode = LoggingMode.ConsoleOnly;
				}
				else if ( string.Compare( arg, "-f" ) == 0 )
				{
					LogMode = LoggingMode.FileOnly;
				}
			}
		}

		private static void WaitForUserToExit()
		{
			Console.Write( "\npress any key to exit" );
			Console.ReadKey( true );
		}
	}
 }
