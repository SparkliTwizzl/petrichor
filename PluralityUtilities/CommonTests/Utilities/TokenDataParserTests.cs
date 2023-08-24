using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.Common.Utilities.Tests
{
	[ TestClass ]
	public class TokenDataParserTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_FlattenTokenTree_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_FlattenTokenTree_Success( Token[] expected, Token input )
		{
			var actual = TokenDataParser.FlattenTokenTree( input );
			CollectionAssert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseTokenFromString_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseTokenFromString_Success( Token expected, string input )
		{
			var actual = TokenDataParser.ParseTokenFromString( input );
			Assert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( InvalidTokenException ) ) ]
		[ DynamicData(
			nameof( Data_ParseTokenFromString_ThrowsInvalidTokenException ),
			DynamicDataSourceType.Method ) ]
		public void Test_ParseTokenFromString_ThrowsInvalidTokenException( string input )
		{
			_ = TokenDataParser.ParseTokenFromString( input );
		}


		private static IEnumerable<object[]> Data_FlattenTokenTree_Success()
		{
			yield return new TestData.DataContainer_FlattenTokenTree_Success()
			{
				Expected = TestData.FlattenedTokenTree,
				Input = TestData.TokenTreeTierA,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> Data_ParseTokenFromString_Success()
		{
			yield return new TestData.DataContainer_ParseTokenFromString_Success
			{
				Expected = TestData.ParsedToken_Standard,
				Input = TestData.RawTokenString_Standard,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTokenFromString_Success
			{
				Expected = TestData.ParsedToken_BlankValue,
				Input = TestData.RawTokenString_BlankTokenValue,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> Data_ParseTokenFromString_ThrowsInvalidTokenException()
		{
			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.RawTokenString_InvalidTokenName,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.RawTokenString_MissingTokenName,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public static Token[] FlattenedTokenTree { get; } =
			{
				TokenTreeTierA,
				TokenTreeTierB,
				TokenTreeTierC,
			};
			public static Token ParsedToken_BlankValue => new()
			{
				Name = TokenName,
			};
			public static Token ParsedToken_Standard => new()
			{
				Name = TokenName,
				Value = TokenValue,
			};
			public static string RawTokenString_InvalidTokenName => $"token name:{ TokenValue }";
			public static string RawTokenString_MissingTokenName => $":{ TokenValue }";
			public static string RawTokenString_BlankTokenValue => $"{ TokenName }:";
			public static string RawTokenString_Standard => $"{ TokenName }:{ TokenValue }";
			public const string TokenName = "Token-Name-0";
			public static Token TokenTreeTierA => new()
			{
				Name = "b-name",
				Value = "b-value",
				Body = new()
				{
					TokenTreeTierB,
				},
			};
			public static Token TokenTreeTierB => new()
			{
				Name = "b-name",
				Value = "b-value",
				Body = new()
				{
					TokenTreeTierC,
				},
			};
			public static Token TokenTreeTierC => new()
			{
				Name = "c-name",
				Value = "c-value",
			};
			public const string TokenValue = "token : value";


			public struct DataContainer_FlattenTokenTree_Success
			{
				public Token[] Expected { get; set; }
				public Token Input { get; set; }


				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input };
				}
			}

			public struct DataContainer_ParseTokenFromString_Success
			{
				public Token Expected { get; set; }
				public string Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Expected, Input };
				}
			}

			public struct DataContainer_ParseTokenFromString_ThrowsException
			{
				public string Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Input };
				}
			}
		}
	}
 }
