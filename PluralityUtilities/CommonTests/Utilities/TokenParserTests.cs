using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Exceptions;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.Common.Utilities.Tests
{
	[ TestClass ]
	public class TokenParserTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}

		[ TestMethod ]
		[ DynamicData( nameof( GetCasesFor_FlattenTokenTree_Success ), DynamicDataSourceType.Method ) ]
		public void Test_FlattenTokenTree_Success( Token[] expected, Token input )
		{
			var actual = TokenParser.FlattenTokenTree( input );
			CollectionAssert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ DynamicData( nameof( GetCasesFor_ParseTokenFromString_Success ), DynamicDataSourceType.Method ) ]
		public void Test_ParseTokenFromString_Success( Token expected, string input )
		{
			var actual = TokenParser.ParseTokenFromString( input );
			Assert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( InvalidTokenException ) ) ]
		[ DynamicData( nameof( GetCasesFor_ParseTokenFromString_ThrowsInvalidTokenException ), DynamicDataSourceType.Method ) ]
		public void Test_ParseTokenFromString_ThrowsInvalidTokenException( string input )
		{
			_ = TokenParser.ParseTokenFromString( input );
		}


		private static IEnumerable<object[]> GetCasesFor_FlattenTokenTree_Success()
		{
			yield return new TestData.DataContainer_FlattenTokenTree_Success()
			{
				Expected = TestData.ValidFlattenedTokenTree,
				Input = TestData.ValidTokenTreeA,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_ParseTokenFromString_Success()
		{
			yield return new TestData.DataContainer_ParseTokenFromString_Success
			{
				Expected = TestData.ValidParsedToken,
				Input = TestData.ValidRawTokenString,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTokenFromString_Success
			{
				Expected = TestData.ValidParsedToken_BlankValue,
				Input = TestData.ValidRawTokenString_BlankTokenValue,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_ParseTokenFromString_ThrowsInvalidTokenException()
		{
			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.InvalidRawTokenString_InvalidTokenName,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.InvalidRawTokenString_MissingTokenName,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public static string InvalidRawTokenString_InvalidTokenName => $"token name:{ ValidTokenValue }";
			public static string InvalidRawTokenString_MissingTokenName => $":{ ValidTokenValue }";
			public static Token[] ValidFlattenedTokenTree { get; } =
			{
				ValidTokenTreeA,
				ValidTokenTreeB,
				ValidTokenTreeC,
			};
			public static Token ValidParsedToken => new()
			{
				Name = ValidTokenName,
				Value = ValidTokenValue,
			};
			public static Token ValidParsedToken_BlankValue => new()
			{
				Name = ValidTokenName,
			};
			public static string ValidTokenName => "Token-Name-0";
			public static string ValidTokenValue => "token : value";
			public static Token ValidTokenTreeA => new()
			{
				Name = "b-name",
				Value = "b-value",
				Body = new()
				{
					ValidTokenTreeB,
				},
			};
			public static Token ValidTokenTreeB => new()
			{
				Name = "b-name",
				Value = "b-value",
				Body = new()
				{
					ValidTokenTreeC,
				},
			};
			public static Token ValidTokenTreeC => new()
			{
				Name = "c-name",
				Value = "c-value",
			};
			public static string ValidRawTokenString => $"{ ValidTokenName }:{ ValidTokenValue }";
			public static string ValidRawTokenString_BlankTokenValue => $"{ ValidTokenName }:";


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
