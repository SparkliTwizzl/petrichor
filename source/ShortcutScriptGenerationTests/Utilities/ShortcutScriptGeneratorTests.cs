﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petrichor.Common.Info;
using Petrichor.ShortcutScriptGeneration.Containers;
using Petrichor.TestShared.Info;
using Petrichor.TestShared.Utilities;


namespace Petrichor.ShortcutScriptGeneration.Utilities.Tests
{
	[ TestClass ]
	public class ShortcutScriptGeneratorTests
	{
		public struct TestData
		{
			public static ShortcutScriptEntry[] Entries => new[]
			{
				new ShortcutScriptEntry( new(){ new( "name", "tag" ) }, "pronoun", "decoration" ),
			};
			public static string[] Templates => new[]
			{
				"::@`tag`:: `name`",
				"::@$&`tag`:: `name` `pronoun` `decoration`",
			};
			public static ShortcutScriptInput Input => new( Metadata, Entries, Templates, Macros );
			public static string[] Macros => new[]
			{
				"::@tag:: name",
				"::@$&tag:: name pronoun decoration",
			};
			public static ShortcutScriptMetadata Metadata => new( TestAssets.DefaultIconFilePath, TestAssets.SuspendIconFilePath, TestAssets.ReloadShortcut, TestAssets.SuspendShortcut );
			public static string[] GeneratedOutputFileContents => new[]
			{
				$"; Generated by { AppInfo.AppNameAndVersion } AutoHotkey shortcut script generator",
				"; https://github.com/SparkliTtwizzl/petrichor",
				"",
				"",
				"#Requires AutoHotkey v2.0",
				"#SingleInstance Force",
				"",
				$"defaultIcon := \"{ Input.Metadata.DefaultIconFilePath }\"",
				$"suspendIcon := \"{ Input.Metadata.SuspendIconFilePath }\"",
				"",
				"",
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
				"; icon handling",
				"; based on code by ntepa on autohotkey.com/boards: https://www.autohotkey.com/boards/viewtopic.php?p=497349#p497349",
				"SuspendC := Suspend.GetMethod( \"Call\" )",
				"Suspend.DefineProp( \"Call\",",
				"	{",
				"		Call:( this, mode := SUSPEND_TOGGLE ) => ( SuspendC( this, mode ), OnSuspend( A_IsSuspended ) )",
				"	})",
				"OnMessage( WM_COMMAND, OnSuspendMsg )",
				"OnSuspendMsg( wparam, * )",
				"{",
				"	if ( wparam = ID_FILE_SUSPEND ) || ( wparam = ID_TRAY_SUSPEND )",
				"	{",
				"		OnSuspend( !A_IsSuspended )",
				"	}",
				"}",
				"",
				"OnSuspend( mode )",
				"{",
				"	scriptIcon := SelectIcon( mode )",
				"	SetIcon( scriptIcon )",
				"}",
				"",
				"SelectIcon( suspendMode )",
				"{",
				"	if ( suspendMode = SUSPEND_ON )",
				"	{",
				"		return suspendIcon",
				"	}",
				"	else if ( suspendMode = SUSPEND_OFF )",
				"	{",
				"		return defaultIcon",
				"	}",
				"	return \"\"",
				"}",
				"",
				"SetIcon( scriptIcon )",
				"{",
				"	if ( FileExist( scriptIcon ) )",
				"	{",
				"		TraySetIcon( scriptIcon,, FREEZE_ICON )",
				"	}",
				"}",
				"",
				"SetIcon( defaultIcon )",
				"",
				"",
				"; script reload / suspend shortcut(s)",
				"#SuspendExempt true",
				$"{ TestAssets.ReloadShortcut }::Reload()",
				$"{ TestAssets.SuspendShortcut }::Suspend( SUSPEND_TOGGLE )",
				"#SuspendExempt false",
				"",
				"",
				"; macros generated from entries and templates",
				"::@tag:: name",
				"::@$&tag:: name pronoun decoration",
			};
		}


		public ShortcutScriptGenerator? scriptGenerator;


		[TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
			scriptGenerator = new ShortcutScriptGenerator( TestData.Input );
		}


		[ TestMethod ]
		public void GenerateScriptTest_Success()
		{
			var outputFile = $@"{ TestDirectories.TestOutputDirectory }\{ nameof( ShortcutScriptGenerator ) }_{ nameof( GenerateScriptTest_Success ) }.ahk";
			scriptGenerator!.GenerateScript( outputFile );

			var expected = TestData.GeneratedOutputFileContents;
			var actual = File.ReadAllLines( outputFile );
			CollectionAssert.AreEqual( expected, actual );
		}
	}
}