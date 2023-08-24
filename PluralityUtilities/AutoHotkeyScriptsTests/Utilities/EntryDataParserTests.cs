using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[TestClass]
	public class EntryDataParserTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		private static IEnumerable<object[]> Data_ParseEntryDataRegion_Success()
		{
			yield return new TestData.DataContainer_ParseEntryDataRegion_Success
			{
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseEntryDataRegion_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseEntryDataRegion_Success(
			Token[] expected, Token input )
		{
			Assert.Fail();
		}


		private static class TestData
		{
			public static Token RegionToken { get; set; } = new();
			public static Token[] ParsedEntryData { get; set; } =
			{
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
		}
	}
}