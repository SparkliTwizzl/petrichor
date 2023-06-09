using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PluralityUtilities.AutoHotkeyScripts.Containers;
using PluralityUtilities.AutoHotkeyScripts.Utilities.Interfaces;
using PluralityUtilities.TestCommon;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{

	[ TestClass ]
	public class AutoHotkeyScriptGeneratorTests
	{
		public static class TestData
		{
			public static readonly Entry[] Entries = new Entry[]
			{
				new Entry( new List<Identity>(){ new Identity( "name", "tag" ) }, "pronoun", "decoration" ),
			};
			public static readonly string[] Templates = new string[]
			{
				"@{tag}::{name}",
				"::@$&{tag}::{name} {pronoun} {decoration}",
			};
			public static readonly Input Input = new( Entries, Templates );
			public static readonly string[] Macros = new string[]
			{
				"::@tag:: name",
				"::@$&tag:: name pronoun decoration",
			};
			public static readonly string[] GeneratedOutputFileContents = new string[]
			{
				"#SingleInstance Force",
				"",
				"::@tag:: name",
				"::@$&tag:: name pronoun decoration",
			};
		}


		public Mock< IInputParser >? InputParserMock { get; set; }
		public IAutoHotkeyScriptGenerator? ScriptGenerator { get; set; }


		[TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();

			InputParserMock = new Mock< IInputParser >();
			InputParserMock.Setup( m => m.ParseInputFile( It.IsAny< string >() ) ).Returns( TestData.Input );

			ScriptGenerator = new AutoHotkeyScriptGenerator( InputParserMock.Object );
		}


		[ TestMethod ]
		[ DataRow( "AutoHotkeyScriptGenerator_Valid.txt" ) ]
		public void GenerateScriptFromInputFileTest_Success( string fileName )
		{
			var inputFile = TestUtilities.LocateInputFile( fileName );
			var outputFile = $"{ TestDirectories.TestOutputDir }{ nameof( AutoHotkeyScriptGenerator ) }_{ nameof( GenerateScriptFromInputFileTest_Success ) }.ahk";
			ScriptGenerator?.GenerateScriptFromInputFile( inputFile, outputFile );

			var expected = TestData.GeneratedOutputFileContents;
			var actual = File.ReadAllLines( outputFile );
			CollectionAssert.AreEqual( expected, actual );
		}
	}
}