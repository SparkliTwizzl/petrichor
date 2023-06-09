using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.AutoHotkeyScripts.Containers;
using PluralityUtilities.Logging;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[TestClass]
	public class InputParserTests
	{
		public static class TestData
		{
			public static Entry[] Entries { get; set; } = new Entry[]
					{
						new Entry
						(
							new List<Identity>
							{
								new Identity("name", "tag")
							},
							"",
							""
						),
					};
			public static string[] Templates { get; set; } = Array.Empty< string >();
			public static Input ParsedInput { get; set; } = new( Entries, Templates );
		}


		public EntryParser EntryParser { get; set; }
		public FieldParser FieldParser { get; set; }
		public InputParser InputParser { get; set; }
		public TemplateParser TemplateParser { get; set; }


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();

			EntryParser = new EntryParser();
			FieldParser = new FieldParser();
			TemplateParser = new TemplateParser();
			InputParser = new InputParser( FieldParser, TemplateParser, EntryParser );
		}


		[ TestMethod ]
		[ DataRow( "InputParser_Valid.txt" ) ]
		public void ParseInputFileTest_Success( string fileName )
		{
			var filePath = TestUtilities.LocateInputFile( fileName );
			var data = File.ReadAllText( filePath );
			Log.WriteLineTimestamped( data );
			var expected = TestData.ParsedInput;
			var actual = InputParser.ParseInputFile( filePath );
			Assert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( FileNotFoundException) ) ]
		[ DataRow( "nonexistent.txt" ) ]
		public void ParseInputFileTest_ThrowsFileNotFoundException( string fileName )
		{
			var filePath = TestUtilities.LocateInputFile( fileName );
			_ = InputParser.ParseInputFile( filePath );
		}
	}
}
