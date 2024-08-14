﻿using Petrichor.Common.Info;
using Petrichor.Common.Utilities;
using Petrichor.Logging;
using Petrichor.Logging.Utilities;
using Petrichor.ShortcutScriptGeneration.Containers;
using Petrichor.ShortcutScriptGeneration.Exceptions;
using System.Text;


namespace Petrichor.ShortcutScriptGeneration.Utilities
{
	public class ScriptGenerator
	{
		private static readonly string DefaultOutputDirectory = ProjectDirectories.OutputDirectory;
		private static readonly string DefaultOutputFileName = $"output.{OutputFileExtension}";
		private const string OutputFileExtension = "ahk";


		private InputData Input { get; set; } = new();
		private string OutputFilePath { get; set; } = string.Empty;
		private int TotalLinesWritten { get; set; } = 0;


		public void Generate( InputData input, string outputFile )
		{
			Input = input;
			var filePathHandler = new FilePathHandler( DefaultOutputDirectory, DefaultOutputFileName );
			filePathHandler.SetFile( outputFile );
			var filePath = Path.ChangeExtension( filePathHandler.FilePath, OutputFileExtension );

			var taskMessage = $"Generate output file \"{filePath}\"";
			Log.Start( taskMessage );

			try
			{
				CreateOutputFile( filePath );
			}
			catch ( Exception exception )
			{
				ExceptionLogger.LogAndThrow( new ScriptGenerationException( $"Failed to generate output file \"{OutputFilePath}\".", exception ) );
			}

			Log.Info( $"Wrote {TotalLinesWritten} total lines to output file." );
			Log.Finish( taskMessage );
		}


		private void CreateOutputFile( string filePath )
		{
			var directory = Path.GetDirectoryName( filePath );
			_ = Directory.CreateDirectory( directory! );
			var file = File.Create( filePath );
			WriteHeaderToFile( file );
			WriteMacrosToFile( file );
			file.Close();
		}

		private static void WriteByteOrderMarkToFile( FileStream file )
		{
			var encoding = Encoding.UTF8;
			file.Write( encoding.GetPreamble() );
			Log.Info( "Wrote byte order mark to output file." );
		}

		private void WriteConstantsToFile( FileStream file )
		{
			var lines = new string[]
			{
				"; constants used for icon handling",
				"FREEZE_ICON := true",
				"ID_FILE_SUSPEND := 65305",
				"ID_TRAY_SUSPEND := 65404",
				"SUSPEND_OFF := 0",
				"SUSPEND_ON := 1",
				"SUSPEND_TOGGLE := -1",
				"WM_COMMAND := 0x111",
				"",
				"",
			};
			WriteLinesToFile( file, lines, " (Constants needed for script execution)" );
		}

		private void WriteControlShortcutsToFile( FileStream file )
		{
			if ( Input.ModuleOptions.ReloadShortcut == string.Empty && Input.ModuleOptions.SuspendShortcut == string.Empty )
			{
				return;
			}

			var lines = new List<string>
			{
				"; script reload / suspend shortcut(s)",
				"#SuspendExempt true"
			};

			if ( Input.ModuleOptions.ReloadShortcut != string.Empty )
			{
				lines.Add( $"{Input.ModuleOptions.ReloadShortcut}::Reload()" );
			}

			if ( Input.ModuleOptions.SuspendShortcut != string.Empty )
			{
				lines.Add( $"{Input.ModuleOptions.SuspendShortcut}::Suspend( SUSPEND_TOGGLE )" );
			}

			lines.Add( "#SuspendExempt false" );
			lines.Add( "" );
			lines.Add( "" );
			WriteLinesToFile( file, lines.ToArray(), " (Script reload/suspend shortcuts)" );
		}

		private void WriteControlStatementsToFile( FileStream file )
		{
			var lines = new string[]
			{
				"#Requires AutoHotkey v2.0",
				"#SingleInstance Force",
				"",
			};
			WriteLinesToFile( file, lines, " (AutoHotkey control statements)" );
		}

		private void WriteGeneratedByMessageToFile( FileStream file )
		{
			var lines = new string[]
			{
				$"; Generated by { AppInfo.AppNameAndVersion } AutoHotkey shortcut script generator",
				"; https://github.com/SparkliTwizzl/petrichor",
				"",
				"",
			};
			WriteLinesToFile( file, lines, " (\"Generated by\" message)" );
		}

		private void WriteHeaderToFile( FileStream file )
		{
			var taskMessage = "Write header to output file";
			Log.Start( taskMessage );
			WriteByteOrderMarkToFile( file );
			WriteGeneratedByMessageToFile( file );
			WriteControlStatementsToFile( file );
			WriteIconFilePathsToFile( file );
			WriteConstantsToFile( file );
			WriteIconHandlingToFile( file );
			WriteControlShortcutsToFile( file );
			Log.Info( $"Wrote {TotalLinesWritten} total lines to output file header." );
			Log.Finish( taskMessage );
		}

		private void WriteIconFilePathsToFile( FileStream file )
		{
			var lines = new string[]
			{
				$"defaultIcon := { Input.ModuleOptions.DefaultIconFilePath }",
				$"suspendIcon := { Input.ModuleOptions.SuspendIconFilePath }",
				"",
				"",
			};
			WriteLinesToFile( file, lines, " (Icon filepaths)" );
		}

		private void WriteIconHandlingToFile( FileStream file )
		{
			var lines = new string[]
			{
				"; icon handling",
				"; based on code by ntepa on autohotkey.com/boards: https://www.autohotkey.com/boards/viewtopic.php?p=497349#p497349",
				"SuspendC := Suspend.GetMethod( \"Call\" )",
				"Suspend.DefineProp( \"Call\",",
				"\t{",
				"\t\tCall:( this, mode := SUSPEND_TOGGLE ) => ( SuspendC( this, mode ), OnSuspend( A_IsSuspended ) )",
				"\t})",
				"OnMessage( WM_COMMAND, OnSuspendMsg )",
				"OnSuspendMsg( wparam, * )",
				"{",
				"\tif ( wparam = ID_FILE_SUSPEND || wparam = ID_TRAY_SUSPEND )",
				"\t{",
				"\t\tOnSuspend( !A_IsSuspended )",
				"\t}",
				"}",
				"",
				"OnSuspend( mode )",
				"{",
				"\tscriptIcon := SelectIcon( mode )",
				"\tSetIcon( scriptIcon )",
				"}",
				"",
				"SelectIcon( suspendMode )",
				"{",
				"\tif ( suspendMode = SUSPEND_ON )",
				"\t{",
				"\t\treturn suspendIcon",
				"\t}",
				"\telse if ( suspendMode = SUSPEND_OFF )",
				"\t{",
				"\t\treturn defaultIcon",
				"\t}",
				"\treturn \"\"",
				"}",
				"",
				"SetIcon( scriptIcon )",
				"{",
				"\tif ( FileExist( scriptIcon ) )",
				"\t{",
				"\t\tTraySetIcon( scriptIcon,, FREEZE_ICON )",
				"\t}",
				"}",
				"",
				"SetIcon( defaultIcon )",
				"",
				"",
			};
			WriteLinesToFile( file, lines, " (Icon handling logic)" );
		}

		private void WriteLineToFile( FileStream file, string line = "" )
		{
			try
			{
				var bytes = Encoding.UTF8.GetBytes( $"{line}\n" );
				file.Write( bytes );
				++TotalLinesWritten;
			}
			catch ( Exception exception )
			{
				ExceptionLogger.LogAndThrow( new FileLoadException( "Failed to write line to output file.", exception ) );
			}
		}

		private void WriteLinesToFile( FileStream file, string[] lines, string message = "" )
		{
			var linesWritten = 0;
			foreach ( var line in lines )
			{
				WriteLineToFile( file, line );
				++linesWritten;
			}
			Log.Info( $"Wrote {linesWritten} lines to output file{message}." );
		}

		private void WriteMacrosToFile( FileStream file )
		{
			var taskMessage = "Write macros to output file";
			Log.Start( taskMessage );
			var lines = new List<string>
			{
				"; macros generated from entries and templates",
			};
			lines.AddRange( Input.Shortcuts );
			WriteLinesToFile( file, lines.ToArray() );
			Log.Finish( taskMessage );
		}
	}
}
