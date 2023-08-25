using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.AutoHotkeyScripts.Exceptions;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Logging;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[TestClass]
	public class FieldDataParserTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		private static IEnumerable<object[]> Data_ParseRegionData_Success()
		{
			yield return new TestData.DataContainer_ParseRegionData_Success()
			{
				Expected = TestData.ParsedFieldDictionary,
				Input = TestData.FieldRegionToken_Standard,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseRegionData_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseRegionData_Success(
			Dictionary<string, string[]> expected, Token input )
		{
			var actual = FieldDataParser.ParseRegionData( input );
			Assert.AreEqual( expected.Count, actual.Count );
			for ( var i = 0; i < actual.Count; ++i )
			{
				var expectedElement = expected.ElementAt( i );
				var actualElement = actual.ElementAt( i );
				Log.Info( $"i = { i }" );
				Log.Info( $"{ nameof( expectedElement ) } =");
				Log.Info( $"{ expectedElement.Key }, [[" );
				foreach ( var value in expectedElement.Value )
				{
					Log.Info( $"    { value }" );
				}
				Log.Info( "]]" );
				Log.Info( $"{ nameof( actualElement ) } =" );
				Log.Info( $"{ actualElement.Key }, [[" );
				foreach ( var value in actualElement.Value )
				{
					Log.Info( $"    { value }" );
				}
				Log.Info( "]]" );
				Log.Separator();

				Assert.AreEqual( expectedElement.Key, actualElement.Key );
				CollectionAssert.AreEqual( expectedElement.Value, actualElement.Value );
			}
		}


		private static IEnumerable<object[]> Data_ParseRegionData_ThrowsDuplicateValueException()
		{
			yield return new TestData.DataContainer_ParseRegionData_ThrowsException()
			{
				Input = TestData.FieldRegionToken_ContainsDuplicateValues,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ ExpectedException( typeof( DuplicateValueException ) ) ]
		[ DynamicData(
			nameof( Data_ParseRegionData_ThrowsDuplicateValueException ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseRegionData_ThrowsDuplicateValueException(
			Token input )
		{
			_ = FieldDataParser.ParseRegionData( input );
		}


		private static class TestData
		{
			public static Dictionary<string, string[]> ParsedFieldDictionary => new()
			{
				{ "fields", new string[] { "a0-value", "a1-value" } },
				{ "a0-value", new string[] { "b0-value" } },
				{ "a1-value", Array.Empty<string>() },
				{ "b0-value", Array.Empty<string>() },
			};
			public const string FieldRegionName = "fields";
			public static Token FieldRegionToken_Standard => new()
			{
				Name = "region",
				Value = FieldRegionName,
				Body = new()
				{
					new()
					{
						Name = "field",
						Value = "a0-value",
						Body = new()
						{
							new()
							{
								Name = "field",
								Value = "b0-value",
							},
						},
					},
					new()
					{
						Name = "field",
						Value = "a1-value",
					},
				},
			};
			public static Token FieldRegionToken_ContainsDuplicateValues => new()
			{
				Name = "region",
				Value = FieldRegionName,
				Body = new()
				{
					new()
					{
						Name = "field",
						Value = "duplicate",
					},
					new()
					{
						Name = "field",
						Value = "duplicate",
					},
				},
			};


			public struct DataContainer_ParseRegionData_Success
			{
				public Dictionary<string, string[]> Expected { get; set; }
				public Token Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input };
				}
			}

			public struct DataContainer_ParseRegionData_ThrowsException
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