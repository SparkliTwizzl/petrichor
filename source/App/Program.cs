﻿using Petrichor.App.Utilities;
using Petrichor.Common.Utilities;
using Petrichor.Logging;


namespace Petrichor.App
{
	static class Program
	{
		private static DateTime startTime;


		static void Main( string[] args )
		{
			startTime = DateTime.Now;
			Console.WriteLine( $"PluralityUtilities v{ AppInfo.CurrentVersion }" );
			CommandLineHandler.ParseArguments( args );
			RuntimeHandler.InitLogging();
			Log.Important( $"PluralityUtilities v{ AppInfo.CurrentVersion }" );
			Log.Important( $"execution started at { startTime.ToString( "yyyy-MM-dd:HH:mm:ss.fffffff" ) }" );
			RuntimeHandler.Execute();
			Log.Important( $"execution finished at { DateTime.Now.ToString( "yyyy-MM-dd:HH:mm:ss.fffffff" ) } (took { ( DateTime.Now - startTime ).TotalSeconds } seconds)" );
			RuntimeHandler.WaitForUserAndExit();
		}
	}
 }
