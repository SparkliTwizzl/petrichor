using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Logging;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[TestClass]
	public class FieldParserTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseFieldData_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseFieldData_Success( Dictionary<string, string[]> expected, Token input )
		{
			var actual = FieldParser.ParseFieldData( input );
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


		private static IEnumerable<object[]> Data_ParseFieldData_Success()
		{
			yield return new TestData.DataContainer_ParseFieldData_Success()
			{
				Expected = TestData.ValidFieldDictionary,
				Input = TestData.ValidInputToken,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public static Dictionary<string, string[]> ValidFieldDictionary => new()
			{
				{ "fields", new string[] { "a0-value", "a1-value" } },
				{ "a0-value", new string[] { "b0-value" } },
				{ "a1-value", Array.Empty<string>() },
				{ "b0-value", Array.Empty<string>() },
			};
			public static Token ValidInputToken => new()
			{
				Name = "region",
				Value = "fields",
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


			public struct DataContainer_ParseFieldData_Success
			{
				public Dictionary<string, string[]> Expected { get; set; }
				public Token Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input };
				}
			}
		}
	}
}