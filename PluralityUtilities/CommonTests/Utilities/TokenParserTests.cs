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


		private static IEnumerable<object[]> GetCasesFor_ParseTokenFromString_Success()
		{
			yield return new TestData.DataContainer_ParseTokenFromString
			{
				Expected = TestData.ValidToken,
				Input = TestData.ValidTokenString,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_ParseTokenFromString_ThrowsInvalidTokenException()
		{
			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.InvalidInput_InvalidTokenName,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.InvalidInput_MissingTokenName,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTokenFromString_ThrowsException
			{
				Input = TestData.InvalidInput_MissingTokenValue,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public static string ValidTokenName => "Token-Name-0";
			public static string ValidTokenValue => "token : value";
			public static string InvalidInput_InvalidTokenName => $"token name:{ ValidTokenValue }";
			public static string InvalidInput_MissingTokenName => $":{ ValidTokenValue }";
			public static string InvalidInput_MissingTokenValue => $"{ ValidTokenName }:";
			public static string ValidTokenString => $"{ ValidTokenName }:{ ValidTokenValue }";
			public static Token ValidToken => new()
			{
				Name = ValidTokenName,
				Value = ValidTokenValue,
			};


			public struct DataContainer_ParseTokenFromString
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
