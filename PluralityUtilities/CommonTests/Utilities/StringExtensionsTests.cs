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
			yield return new TestData.DataContainer_FilePathMethods_AllCases
			{
				Expected = TestData.Expected_GetDirectory,
				Input = TestData.Input_FilePaths,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_GetDirectory_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_GetDirectory_Success(
			string[] expected, string[] input )
		{
			var outputData = new List<string>();
			foreach ( var line in input )
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
			yield return new TestData.DataContainer_FilePathMethods_AllCases
			{
				Expected = TestData.Expected_GetFileName,
				Input = TestData.Input_FilePaths,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_GetFileName_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_GetFileName_Success(
			string[] expected, string[] input )
		{
			var outputData = new List<string>();
			foreach ( var line in input )
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
			yield return new TestData.DataContainer_FilePathMethods_AllCases
			{
				Expected = TestData.Expected_RemoveExtension,
				Input = TestData.Input_FilePaths,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_RemoveFileExtension_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_RemoveFileExtension_Success(
			string[] expected, string[] input )
		{
			var outputData = new List<string>();
			foreach ( var line in input )
			{
				var result = line.RemoveFileExtension();
				outputData.Add( result );
				Log.Info( $"\"{ result }\"" );
			}
			var actual = outputData.ToArray();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static IEnumerable<object[]> Data_IndexOfUnescaped_Success()
		{
			yield return new TestData.DataContainer_IndexOfUnescaped_AllCases
			{
				Expected = TestData.Expected_IndexOfUnescaped,
				Input_Source = TestData.Input_EscapedStrings,
				Input_ToFind = TestData.Input_ToFind,
				Input_Escapement = TestData.Input_Escapement,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_IndexOfUnescaped_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_IndexOfUnescaped_Success(
			int[] expected, string[] source, char toFind, char escapement )
		{
			var outputData = new List<int>();
			foreach ( var line in source )
			{
				var result = line.IndexOfUnescaped( toFind, escapement );
				outputData.Add( result );
				Log.Info( $"{ result }" );
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
			public static int[] Expected_IndexOfUnescaped => new[]
			{
				-1,
				-1,
				-1,
				0,
				1,
				-1,
				1,
			};
			public static string[] Input_FilePaths => new[]
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
			public static string[] Input_EscapedStrings => new[]
			{
				string.Empty,
				"",
				" ",
				"a",
				" a",
				"\a",
				"\aa",
			};
			public static char Input_ToFind => 'a';
			public static char Input_Escapement => '\\';


			public struct DataContainer_FilePathMethods_AllCases
			{
				public string[] Expected { get; set; }
				public string[] Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input };
				}
			}

			public struct DataContainer_IndexOfUnescaped_AllCases
			{
				public int[] Expected { get; set; }
				public char Input_Escapement { get; set; }
				public string[] Input_Source { get; set; }
				public char Input_ToFind { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input_Source, Input_ToFind, Input_Escapement };
				}
			}
		}
	}
}