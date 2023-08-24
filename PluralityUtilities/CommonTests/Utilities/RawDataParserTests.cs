using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.Common.Utilities.Tests
{
	[ TestClass ]
	public class RawDataParserTests
	{
		RawDataParser RawDataParser { get; set; } = new RawDataParser();


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
			RawDataParser = new RawDataParser();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseRawData_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseRawData_Success( Token expected, string[] inputData )
		{
			var actual = RawDataParser.ParseRawData( inputData );
			Assert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( IndentImbalanceException ) ) ]
		[ DynamicData(
			nameof( Data_ParseRawData_ThrowsIndentImbalanceException ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseRawData_ThrowsIndentImbalanceException( Token expected, string[] inputData )
		{
			var actual = RawDataParser.ParseRawData( inputData );
			Assert.AreEqual( expected, actual );
		}


		private static IEnumerable<object[]> Data_ParseRawData_Success()
		{
			yield return new TestData.DataContainer_ParseRawData_Success
			{
				Expected = TestData.ParsedData,
				InputData = TestData.RawData_Standard,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> Data_ParseRawData_ThrowsIndentImbalanceException()
		{
			yield return new TestData.DataContainer_ParseRawData_Success
			{
				InputData = TestData.RawData_ImbalancedOpenBracket,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public static Token ParsedData => new Token()
			{
				Name = "",
				Value = "",
				Body = new List<Token>
				{
					new Token()
					{
						Name = "a0-name",
						Value = "a0-value",
						Body = new List<Token>
						{
							new Token()
							{
								Name = "b0-name",
								Value = "b0-value",
								Body = new List<Token>
								{
									new Token()
									{
										Name = "c0-name",
										Value = "c0-value",
									},
									new Token()
									{
										Name = "c1-name",
										Value = "c1-value",
									},
								},
							},
							new Token()
							{
								Name = "b1-name",
								Value = "b1-value",
							},
						},
					},
					new Token()
					{
						Name = "a1-name",
						Value = "a1-value",
					},
				},
			};
			public static string[] RawData_ImbalancedCloseBracket => new string[]
			{
				"a0-name:a0-value",
				"}",
			};
			public static string[] RawData_ImbalancedOpenBracket => new string[]
			{
				"a0-name:a0-value",
				"{",
			};
			public static string[] RawData_Standard => new string[]
			{
				"a0-name:a0-value",
				"{",
				"	b0-name:b0-value",
				"	{",
				"		c0-name:c0-value",
				"		c1-name:c1-value",
				"	}",
				"	b1-name:b1-value",
				"}",
				"a1-name:a1-value",
			};



			public struct DataContainer_ParseRawData_Success
			{
				public Token Expected { get; set; }
				public string[] InputData { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, InputData };
				}
			}

			public struct DataContainer_ParseRawData_ThrowsException
			{
				public string[] InputData { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { InputData };
				}
			}
		}
	}
}
