using System.Reflection;

using PluralityUtilities.Logging.Enums;


namespace PluralityUtilities.Logging
{
	public static class Logger
	{
		public static bool IsLoggingEnabled() => _mode != LoggingMode.Disabled;
		public static bool IsLoggingToConsoleEnabled() => _mode == LoggingMode.ConsoleOnly || _mode == LoggingMode.All;
		public static bool IsLoggingToFileEnabled() => _mode == LoggingMode.FileOnly || _mode == LoggingMode.All;


		private static readonly string _defaultLogFolder = $"{ Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location ) }/log/";
		private static readonly string _defaultLogFileName = $"{ DateTime.Now.ToString( "yyyy-MM-dd_HH-mm-ss" ) }.log";
		private static LoggingMode _mode = LoggingMode.Disabled;
		private static string _logFolder = string.Empty;
		private static string _logFileName = string.Empty;
		private static string _logFilePath = string.Empty;


		public static void Disable()
		{
			_mode = LoggingMode.Disabled;
		}

		public static void EnableAll()
		{
			_mode = LoggingMode.All;
		}

		public static void EnableConsoleOnly()
		{
			_mode = LoggingMode.ConsoleOnly;
		}

		public static void EnableFileOnly()
		{
			_mode = LoggingMode.FileOnly;
		}

		public static void LogError( Exception exception )
		{
			WriteLine( $"AN ERROR OCCURRED : [[ { exception } ]]" );
		}

		public static void LogError( Exception exception, string message )
		{
			WriteLine( $"AN ERROR OCCURRED : { message } [[ { exception } ]]" );
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

		public static void Write( string message = "" )
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

			var timestampedMessage = $"{ DateTime.Now.ToString( "yyyy-MM-dd:HH:mm:ss" ) } - { message }";
			if ( IsLoggingToConsoleEnabled() )
			{
				Console.Write( timestampedMessage );
			}
			if ( IsLoggingToFileEnabled() )
			{
				using ( StreamWriter logFile = File.AppendText( _logFilePath ) )
				{
					logFile.Write( timestampedMessage );
				}
			}
		}

		public static void WriteLine( string message = "" )
		{
			Write( $"{ message }\n" );
		}


		private static void SetLogFilePath()
		{
			_logFilePath = $"{ _logFolder }{ _logFileName }";
		}
	}
}
