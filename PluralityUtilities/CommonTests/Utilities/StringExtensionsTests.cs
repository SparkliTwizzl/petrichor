using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluralityUtilities.Logging;
using PluralityUtilities.TestCommon.Utilities;

namespace PluralityUtilities.Common.Utilities.Tests
{
	[ TestClass ]
	public class StringExtensionsTests
	{


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		private static IEnumerable<object[]> Data_GetDirectory_Success()
		{
			yield return new TestData.DataContainer_AllMethods_AllCases
			{
				Expected = TestData.Expected_GetDirectory,
				InputData = TestData.InputData,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_GetDirectory_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_GetDirectory_Success(
			string[] expected, string[] inputData )
		{
			var outputData = new List<string>();
			foreach ( var line in inputData )
			{
				var result = line.GetDirectory();
				outputData.Add( result );
				Log.Info( $"\"{ result }\"" );
			}
			var actual = outputData.ToArray();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static IEnumerable<object[]> Data_GetFileName_Success()
		{
			yield return new TestData.DataContainer_AllMethods_AllCases
			{
				Expected = TestData.Expected_GetFileName,
				InputData = TestData.InputData,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_GetFileName_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_GetFileName_Success(
			string[] expected, string[] inputData )
		{
			var outputData = new List<string>();
			foreach ( var line in inputData )
			{
				var result = line.GetFileName();
				outputData.Add( result );
				Log.Info( $"\"{ result }\"" );
			}
			var actual = outputData.ToArray();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static IEnumerable<object[]> Data_RemoveFileExtension_Success()
		{
			yield return new TestData.DataContainer_AllMethods_AllCases
			{
				Expected = TestData.Expected_RemoveExtension,
				InputData = TestData.InputData,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_RemoveFileExtension_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_RemoveFileExtension_Success(
			string[] expected, string[] inputData )
		{
			var outputData = new List<string>();
			foreach ( var line in inputData )
			{
				var result = line.RemoveFileExtension();
				outputData.Add( result );
				Log.Info( $"\"{ result }\"" );
			}
			var actual = outputData.ToArray();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static class TestData
		{
			public static string[] Expected_GetDirectory => new[]
			{
				"z:\\folder\\",
				@"z:\folder\",
				"z:/folder/",
				"z:\\folder/",
				@"z:\folder/",
				"z:\\folder\\",
				@"z:\folder\",
				"z:/folder/",
				"z:\\folder/",
				@"z:\folder/",
				string.Empty,
				string.Empty,
				"",
				string.Empty,
			};
			public static string[] Expected_GetFileName => new[]
			{
				"file.ext",
				"file.ext",
				"file.ext",
				"file.ext",
				"file.ext",
				"",
				"",
				"",
				"",
				"",
				"file.ext",
				"text",
				"",
				string.Empty,
			};
			public static string[] Expected_RemoveExtension => new[]
			{
				"z:\\folder\\file",
				@"z:\folder\file",
				"z:/folder/file",
				"z:\\folder/file",
				@"z:\folder/file",
				"z:\\folder\\",
				@"z:\folder\",
				"z:/folder/",
				"z:\\folder/",
				@"z:\folder/",
				"file",
				"text",
				"",
				string.Empty,
			};
			public static string[] InputData => new[]
			{
			"z:\\folder\\file.ext",
			@"z:\folder\file.ext",
			"z:/folder/file.ext",
			"z:\\folder/file.ext",
			@"z:\folder/file.ext",
			"z:\\folder\\",
			@"z:\folder\",
			"z:/folder/",
			"z:\\folder/",
			@"z:\folder/",
			"file.ext",
			"text",
			"",
			string.Empty,
			};


			public struct DataContainer_AllMethods_AllCases
			{
				public string[] Expected { get; set; }
				public string[] InputData { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, InputData };
				}
			}
		}
	}
}