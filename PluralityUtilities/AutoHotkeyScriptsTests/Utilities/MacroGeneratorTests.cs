using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[ TestClass ]
	public class MacroGeneratorTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		private static IEnumerable<object[]> Data_GenerateMacros_Success()
		{
			yield return new TestData.DataContainer_GenerateMacros_Success
			{
			}.ToObjectArray();
		}

		[TestMethod ]
		[ DynamicData(
			nameof( Data_GenerateMacros_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_GenerateMacros_Success()
		{
			Assert.Fail();
		}


		private static class TestData
		{
			public struct DataContainer_GenerateMacros_Success
			{
				public string[] Expected { get; set; }
				public Token[] Input { get; set; }

				public object[] ToObjectArray() => new object[]
				{
						Expected,
						Input,
				};
			}
		}
	}
}