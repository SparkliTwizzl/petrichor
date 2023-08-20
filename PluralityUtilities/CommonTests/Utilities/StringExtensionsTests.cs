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


		[ TestMethod ]
		[ DynamicData( nameof( GetCasesFor_GetDirectory ), DynamicDataSourceType.Method ) ]
		public void Test_GetDirectory( string[] expected, string[] inputData )
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

		[ TestMethod ]
		[ DynamicData( nameof( GetCasesFor_GetFileName ), DynamicDataSourceType.Method ) ]
		public void Test_GetFileName( string[] expected, string[] inputData )
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

		[ TestMethod ]
		[ DynamicData( nameof( GetCasesFor_RemoveExtension ), DynamicDataSourceType.Method ) ]
		public void Test_RemoveFileExtension( string[] expected, string[] inputData )
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


		private static IEnumerable<object[]> GetCasesFor_GetDirectory()
		{
			yield return new TestData.DataContainer_AllMethods_AllCases
			{
				Expected = TestData.Expected_GetDirectory,
				InputData = TestData.InputData,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_GetFileName()
		{
			yield return new TestData.DataContainer_AllMethods_AllCases
			{
				Expected = TestData.Expected_GetFileName,
				InputData = TestData.InputData,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_RemoveExtension()
		{
			yield return new TestData.DataContainer_AllMethods_AllCases
			{
				Expected = TestData.Expected_RemoveExtension,
				InputData = TestData.InputData,
			}.ToObjectArray();
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