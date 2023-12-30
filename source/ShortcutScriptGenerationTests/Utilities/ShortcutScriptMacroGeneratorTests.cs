﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petrichor.ShortcutScriptGeneration.Containers;
using Petrichor.TestShared.Info;
using Petrichor.TestShared.Utilities;


namespace Petrichor.ShortcutScriptGeneration.Utilities.Tests
{
	[TestClass]
	public class ShortcutScriptMacroGeneratorTests
	{
		public struct TestData
		{
			public static ScriptEntry[] Entries => new[]
			{
				new ScriptEntry( new List<ShortcutScriptIdentity>(){ new( "name", "tag" ) }, "pronoun", "decoration" ),
			};
			public static ScriptInput Input => new( ModuleOptions, Entries, Templates );
			public static string[] Macros => new[]
			{
				"::@tag:: name",
				"::@$&tag:: name pronoun decoration",
			};
			public static ScriptModuleOptions ModuleOptions => new( TestAssets.DefaultIconFileName, TestAssets.SuspendIconFilePath, TestAssets.ReloadShortcut, TestAssets.SuspendShortcut );
			public static string[] Templates => new[]
			{
				"::@`tag`:: `name`",
				"::@$&`tag`:: `name` `pronoun` `decoration`",
			};
		}


		public ShortcutScriptMacroGenerator? macroGenerator;


		[TestInitialize]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
			macroGenerator = new ShortcutScriptMacroGenerator();
		}


		[TestMethod]
		public void Generate_Test_Success()
		{
			var expected = TestData.Macros;
			var actual = macroGenerator!.Generate( TestData.Input ).ToArray();
			CollectionAssert.AreEqual( expected, actual );
		}
	}
}
