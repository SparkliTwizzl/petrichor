﻿using PluralityUtilities.AutoHotkeyScripts.Utilities;
using PluralityUtilities.Common;
using PluralityUtilities.Logging;
using PluralityUtilities.Logging.Enums;


namespace PluralityUtilities.App
{
	static class Program
	{
		private static string _inputFilePath = string.Empty;
		private static LogMode _logMode = LogMode.Disabled;
		private static string _outputFilePath = string.Empty;
		private static DateTime _startTime;
		private const string _version = "0.4";


		static void Main(string[] args)
		{
			_startTime = DateTime.Now;
			Console.WriteLine($"PluralityUtilities v{_version}");
			ParseArgs(args);
			InitLogging();
			if (_inputFilePath == string.Empty)
			{
				return;
			}
			Log.WriteLineTimestamped($"PluralityUtilities v{_version}; execution started at {_startTime}");
			ParseInputAndGenerateAutoHotkeyScript();
			Log.WriteLineTimestamped($"execution finished in {(DateTime.Now - _startTime).TotalSeconds} seconds");
			Console.Write("press any key to exit");
			Console.ReadKey(true);
		}


		private static void InitLogging()
		{
			switch (_logMode)
			{
				case LogMode.Basic:
					Log.EnableBasic();
					Log.SetLogFolder(ProjectDirectories.LogDir);
					Console.WriteLine("logging is enabled");
					break;
				case LogMode.Verbose:
					Log.EnableVerbose();
					Log.SetLogFolder(ProjectDirectories.LogDir);
					Console.WriteLine("verbose logging is enabled");
					break;
				default:
					Console.WriteLine("logging is disabled");
					break;
			}
		}

		private static void ParseArgs(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("pass path to input file as arg0; pass name of output file as arg1; pass \"-l\" as arg2 to enable logging; pass \"-v\" as arg2 to enable verbose logging");
				return;
			}
			_inputFilePath = args[0];
			_outputFilePath = args[1];
			if (args.Length > 2)
			{
				var arg = args[2];
				if (arg == "-l")
				{
					_logMode = LogMode.Basic;
				}
				else if (arg == "-v")
				{
					_logMode = LogMode.Verbose;
				}
			}
		}

		private static void ParseInputAndGenerateAutoHotkeyScript()
		{
			try
			{
				InputParser parser = new InputParser();
				parser.ParseFile(_inputFilePath);
				AutoHotkeyScriptGenerator scriptGenerator = new AutoHotkeyScriptGenerator();
				scriptGenerator.Generate(parser.People, _outputFilePath);
				var successMessage = "generating script succeeded";
				Console.WriteLine(successMessage);
				Log.WriteLineTimestamped(successMessage);
			}
			catch (Exception ex)
			{
				var errorMessage = $"generating script failed with error: {ex.Message}";
				if (_logMode != LogMode.Verbose)
				{
					Console.WriteLine(errorMessage);
				}
				Log.WriteLineTimestamped(errorMessage);
			}
		}
	}
}
