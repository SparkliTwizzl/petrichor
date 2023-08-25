using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Logging;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[TestClass]
	public class EntryDataParserTests
	{
		private EntryDataParser TestEntryDataParser { get; set; } = new EntryDataParser();


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
			TestEntryDataParser = new EntryDataParser( TestData.FieldDictionary );
		}


		private static IEnumerable<object[]> Data_ParseEntryDataRegion_Success()
		{
			yield return new TestData.DataContainer_ParseEntryDataRegion_Success
			{
				Expected = TestData.ParsedEntryData,
				Input = TestData.EntryRegionToken_Standard,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseEntryDataRegion_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseEntryDataRegion_Success(
			Token[] expected, Token input )
		{
			var actual = TestEntryDataParser.ParseEntryDataRegion( input );
			Log.Separator();
			Log.Info( "expected = [[" );
			foreach ( var token in expected )
			{
				Log.Info( $"'{ token.Name }', '{ token.Value }'" );
			}
			Log.Info( "]]" );
			Log.Separator();
			Log.Info( "actual = [[" );
			foreach ( var token in actual )
			{
				Log.Info( $"'{ token.Name }', '{ token.Value }'" );
			}
			Log.Info( "]]" );
			Log.Separator();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static IEnumerable<object[]> Data_ParseEntryDataRegion_ThrowsInvalidDataException()
		{
			yield return new TestData.DataContainer_ParseEntryDataRegion_ThrowsException
			{
				Input = TestData.EntryRegionToken_HasInvalidStructure,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ ExpectedException( typeof( InvalidDataException ) ) ]
		[ DynamicData(
			nameof( Data_ParseEntryDataRegion_ThrowsInvalidDataException ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseEntryDataRegion_ThrowsInvalidDataException(
			Token input )
		{
			_ = TestEntryDataParser.ParseEntryDataRegion( input );
		}


		private static class TestData
		{
			public const string EntryRegionName = "entries";
			public static Token EntryRegionToken_Standard => new()
			{
				Name = "region",
				Value = EntryRegionName,
				Body = new()
				{
					EntryTokenA0,
					EntryTokenA1,
				}
			};
			public static Token EntryRegionToken_HasInvalidStructure => new()
			{
				Name = "region",
				Value = EntryRegionName,
				Body = new()
				{
					EntryTokenB0,
				}
			};
			public static Token EntryTokenA0 => new()
			{
				Name = "a0",
				Value = "a0-value",
				Body = new()
				{
					EntryTokenB0,
					EntryTokenB1,
				},
			};
			public static Token EntryTokenA1 => new()
			{
				Name = "a1",
				Value = "a1-value",
			};
			public static Token EntryTokenB0 => new()
			{
				Name = "b0",
				Value = "b0-value",
				Body = new()
				{
					EntryTokenC0,
				},
			};
			public static Token EntryTokenB1 => new()
			{
				Name = "b1",
				Value = "b1-value",
			};
			public static Token EntryTokenC0 => new()
			{
				Name = "c0",
				Value = "c0-value",
			};
			public static Dictionary<string, string[]> FieldDictionary => new()
			{
				{ "region", new string[] { "a0", "a1" } },
				{ "a0", new string[] { "b0", "b1" } },
				{ "a1", Array.Empty<string>() },
				{ "b0", new string[] { "c0" } },
				{ "b1", Array.Empty<string>() },
				{ "c0", Array.Empty<string>() },
			};
			public static Token[] ParsedEntryData => new Token[]
			{
				EntryTokenA0,
				EntryTokenA1,
				EntryTokenB0,
				EntryTokenB1,
				EntryTokenC0,
			};


			public struct DataContainer_ParseEntryDataRegion_Success
			{
				public Token[] Expected { get; set; }
				public Token Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input };
				}
			}

			public struct DataContainer_ParseEntryDataRegion_ThrowsException
			{
				public Token Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Input };
				}
			}
		}
	}
}