﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petrichor.Common.Info;
using Petrichor.ShortcutScriptGeneration.Containers;
using Petrichor.ShortcutScriptGeneration.Info;
using Petrichor.TestShared.Info;
using Petrichor.TestShared.Utilities;


namespace Petrichor.ShortcutScriptGeneration.Utilities.Tests
{
	[TestClass]
	public class MacroGeneratorTests
	{
		public struct TestData
		{
			public static ScriptEntry[] Entries => new[]
			{
				new ScriptEntry(
					EntryID,
					new List<ScriptIdentity>(){ new( EntryName, EntryTag ) },
					new ScriptIdentity( EntryLastName, EntryLastTag ),
					EntryPronoun,
					EntryColor,
					EntryDecoration
				),
			};
			public static string EntryID => "ID";
			public static string EntryColor => "COLOR";
			public static string EntryDecoration => "DECORATION";
			public static string EntryName => "NAME";
			public static string EntryLastName => "LAST_NAME";
			public static string EntryLastTag => "LAST_TAG";
			public static string EntryPronoun => "PRONOUN";
			public static string EntryTag => "TAG";
			public static ScriptInput Input => new( ModuleOptions, Entries, Templates );
			public static string[] Macros => new[]
			{
				$"::{ CommonSyntax.EscapeChar }{ EntryTag }{ EntryLastTag }:: { CommonSyntax.FindTokenOpenChar }{ EntryID }{ CommonSyntax.FindTokenCloseChar } { EntryName } { EntryLastName } { EntryPronoun } { EntryColor } { EntryDecoration } `",
			};
			public static ScriptModuleOptions ModuleOptions => new( TestAssets.DefaultIconFileName, TestAssets.SuspendIconFilePath, TestAssets.ReloadShortcut, TestAssets.SuspendShortcut );
			public static string Template => $"::{ CommonSyntax.EscapeCharStandin }{ ShortcutScriptSyntax.TemplateFindTagString }{ ShortcutScriptSyntax.TemplateFindLastTagString }:: { CommonSyntax.FindTokenOpenCharStandin }{ ShortcutScriptSyntax.TemplateFindIDString }{ CommonSyntax.FindTokenCloseCharStandin } { ShortcutScriptSyntax.TemplateFindNameString } { ShortcutScriptSyntax.TemplateFindLastNameString } { ShortcutScriptSyntax.TemplateFindPronounString } { ShortcutScriptSyntax.TemplateFindColorString } { ShortcutScriptSyntax.TemplateFindDecorationString } `";
			public static string[] Templates => new[]
			{
				Template,
			};
		}


		public MacroGenerator? generator;


		[TestInitialize]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
			generator = new();
		}


		[TestMethod]
		public void Generate_Test_Success()
		{
			var expected = TestData.Macros;
			var actual = generator!.Generate( TestData.Input );
			CollectionAssert.AreEqual( expected, actual );
		}
	}
}
