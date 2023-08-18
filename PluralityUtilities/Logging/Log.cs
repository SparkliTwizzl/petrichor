using System.Reflection;

using PluralityUtilities.Logging.Enums;


namespace PluralityUtilities.Logging
{
	public static class Log
	{
		private static readonly string _defaultLogFolder = $"{ Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) }/log/";
		private static readonly string _defaultLogFileName = $"{ DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss" ) }.log";
		private static LoggingMode _mode = LoggingMode.Disabled;
		private static string _logFolder = string.Empty;
		private static string _logFileName = string.Empty;
		private static string _logFilePath = string.Empty;


		public static bool IsLoggingEnabled => _mode != LoggingMode.Disabled;
		public static bool IsLoggingToConsoleEnabled => _mode == LoggingMode.ConsoleOnly || _mode == LoggingMode.All;
		public static bool IsLoggingToFileEnabled => _mode == LoggingMode.FileOnly || _mode == LoggingMode.All;


		public static void Disable()
		{
			_mode = LoggingMode.Disabled;
		}

		public static void EnableForAll()
		{
			_mode = LoggingMode.All;
		}

		public static void EnableForConsoleOnly()
		{
			_mode = LoggingMode.ConsoleOnly;
		}

		public static void EnableForFileOnly()
		{
			_mode = LoggingMode.FileOnly;
		}

		public static void Error( string message )
		{
			WriteToLog( $"ERROR : { message }" );
		}

		public static void Exception( Exception exception )
		{
			WriteToLog( $"EXCEPTION : { exception.Message } [[ { exception } ]]" );
		}

		public static void Exception( Exception exception, string message )
		{
			WriteToLog( $"EXCEPTION : { message } // { exception.Message } [[ { exception } ]]" );
		}

		public static void Info( string message )
		{
			WriteToLog( $"INFO : { message }" );
		}

		public static void Warning( string message )
		{
			WriteToLog( $"WARNING : { message }" );
		}

		public static void Separator()
		{
			WriteToLog( "----------------------------------------------------------------------------------------------------" );
		}

		public static void SetLogFileName( string filename )
		{
			_logFileName = filename;
			SetLogFilePath();
		}

		public static void SetLogFolder( string folder )
		{
			_logFolder = folder;
			var lastChar = folder[ folder.Length - 1 ];
			if ( lastChar != '\\' && lastChar != '/' )
			{
				_logFolder += '/';
			}
			Directory.CreateDirectory( _logFolder );
			SetLogFilePath();
		}


		private static void SetLogFilePath()
		{
			_logFilePath = $"{ _logFolder }{ _logFileName }";
		}

		private static void WriteToLog( string message = "" )
		{
			if (_mode == LoggingMode.Disabled)
			{
				return;
			}

			if ( _logFolder == "" )
			{
				SetLogFolder( _defaultLogFolder );
			}
			if ( _logFileName == "" )
			{
				SetLogFileName( _defaultLogFileName );
			}

			var timestampedMessage = $"[{ DateTime.Now.ToString( "yyyy-MM-dd:HH:mm:ss.fff" ) }] { message }\n";
			if ( IsLoggingToConsoleEnabled )
			{
				Console.Write( timestampedMessage );
			}
			if ( IsLoggingToFileEnabled )
			{
				using ( StreamWriter logFile = File.AppendText( _logFilePath ) )
				{
					logFile.Write( timestampedMessage );
				}
			}
		}
	}
}
