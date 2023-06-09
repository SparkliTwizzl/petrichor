using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.AutoHotkeyScripts.Containers;
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
				"::@`tag`:: `name`",
				"::@$&`tag`:: `name` `pronoun` `decoration`",
			};
			public static readonly Input Input = new Input( Entries, Templates );
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


		public AutoHotkeyScriptGenerator ScriptGenerator { get; set; }
		public EntryParser EntryParser { get; set; }
		public FieldParser FieldParser { get; set; }
		public InputParser InputParser { get; set; }
		public TemplateParser TemplateParser { get; set; }


		[TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();

			FieldParser = new FieldParser();
			EntryParser = new EntryParser();
			TemplateParser = new TemplateParser();
			InputParser = new InputParser( FieldParser, TemplateParser, EntryParser );
			ScriptGenerator = new AutoHotkeyScriptGenerator( InputParser );
		}


		[ TestMethod ]
		[ DataRow( "AutoHotkeyScriptGenerator_Valid.txt" ) ]
		public void GenerateScriptFromInputFileTest_Success( string fileName )
		{
			var inputFile = TestUtilities.LocateInputFile( fileName );
			var outputFile = $"{ TestDirectories.TestOutputDir }{ nameof( AutoHotkeyScriptGenerator ) }_{ nameof( GenerateScriptFromInputFileTest_Success ) }.ahk";
			ScriptGenerator.GenerateScriptFromInputFile( inputFile, outputFile );

			var expected = TestData.GeneratedOutputFileContents;
			var actual = File.ReadAllLines( outputFile );
			CollectionAssert.AreEqual( expected, actual );
		}
	}
}