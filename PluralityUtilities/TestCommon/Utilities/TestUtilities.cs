using PluralityUtilities.Logging;

namespace PluralityUtilities.TestCommon.Utilities
{
	public static class TestUtilities
	{
		public static void InitializeLoggingForTests()
		{
			Logger.SetLogFolder( TestDirectories.TestLogDir );
			Logger.SetLogFileName( DateTime.Now.ToString( "test_yyyy-MM-dd_hh-mm-ss.log" ) );
			Logger.EnableAll();
		}

		public static string LocateInputFile( string fileName )
		{
			return $"{ TestDirectories.TestInputDir }{ fileName }";
		}
	}
}
