using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PluralityUtilities.AutoHotkeyScripts.Containers;
using PluralityUtilities.AutoHotkeyScripts.Utilities.Interfaces;
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
							new List< Identity >
							{
								new Identity( "name", "tag" )
							},
							"",
							""
						),
					};
			public static readonly string[] Templates = new string[] { "@{tag}::{name}" };
			public static readonly Input ParsedInput = new( Entries, Templates );
		}


		public Mock< IEntryParser >? EntryParserMock { get; set; }
		public Mock< IFieldParser >? FieldParserMock { get; set; }
		public IInputParser? InputParser { get; set; }
		public Mock< ITemplateParser >? TemplateParserMock { get; set; }
		int i;


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();

			i = 1;

			EntryParserMock = new Mock< IEntryParser >();
			EntryParserMock.Setup( m => m.ParseEntriesFromData( It.IsAny< string[] >(), ref i ) ).Returns( TestData.Entries );

			FieldParserMock = new Mock< IFieldParser >();

			TemplateParserMock = new Mock< ITemplateParser >();
			TemplateParserMock.Setup( m => m.ParseTemplatesFromData( It.IsAny< string[] >(), ref i ) ).Returns( TestData.Templates );

			InputParser = new InputParser( FieldParserMock.Object, TemplateParserMock.Object, EntryParserMock.Object );
		}


		[ TestMethod ]
		[ DataRow( "InputParser_Valid.txt" ) ]
		public void ParseInputFileTest_Success( string fileName )
		{
			var filePath = TestUtilities.LocateInputFile( fileName );
			var data = File.ReadAllText( filePath );
			Log.Info( data );

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
