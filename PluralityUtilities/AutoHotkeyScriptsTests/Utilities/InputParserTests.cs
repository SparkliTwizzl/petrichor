using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
			public static readonly Entry[] Entries = new Entry[]
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
			public static readonly string[] Templates = Array.Empty< string >();
			public static readonly Input ParsedInput = new( Entries, Templates );
		}


		public Mock< EntryParser >? EntryParserMock { get; set; }
		public Mock< FieldParser >? FieldParserMock { get; set; }
		public Mock< TemplateParser >? TemplateParserMock { get; set; }
		public InputParser? InputParser { get; set; }


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();

			EntryParserMock = new Mock< EntryParser >();
			FieldParserMock = new Mock< FieldParser >();
			TemplateParserMock = new Mock< TemplateParser >();
			InputParser = new InputParser( FieldParserMock.Object, TemplateParserMock.Object, EntryParserMock.Object );
		}


		[ TestMethod ]
		[ DataRow( "InputParser_Valid.txt" ) ]
		public void ParseInputFileTest_Success( string fileName )
		{
			var filePath = TestUtilities.LocateInputFile( fileName );
			var data = File.ReadAllText( filePath );
			Log.WriteLineTimestamped( data );
			var expected = TestData.ParsedInput;
			var actual = InputParser?.ParseInputFile( filePath );
			Assert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( FileNotFoundException) ) ]
		[ DataRow( "nonexistent.txt" ) ]
		public void ParseInputFileTest_ThrowsFileNotFoundException( string fileName )
		{
			var filePath = TestUtilities.LocateInputFile( fileName );
			_ = InputParser?.ParseInputFile( filePath );
		}
	}
}
