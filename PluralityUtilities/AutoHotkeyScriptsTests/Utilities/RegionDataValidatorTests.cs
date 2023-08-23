using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.AutoHotkeyScripts.Exceptions;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[ TestClass ]
	public class RegionDataValidatorTests
	{
		[TestInitialize]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}

		[ TestMethod ]
		[ DynamicData( nameof( GetCasesFor_RejectDuplicateTokenValues_Success ), DynamicDataSourceType.Method ) ]
		public void Test_RejectDuplicateTokenValues_Success( Token[] input )
		{
			RegionDataValidator.RejectDuplicateTokenValues( input, TestData.RegionName );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( DuplicateValueException ) ) ]
		[ DynamicData( nameof( GetCasesFor_RejectDuplicateTokenValues_ThrowsDuplicateValueException ), DynamicDataSourceType.Method ) ]
		public void Test_RejectDuplicateTokenValues_ThrowsDuplicateValueException( Token[] input )
		{
			RegionDataValidator.RejectDuplicateTokenValues( input, TestData.RegionName );
		}

		[TestMethod ]
		[ DynamicData( nameof( GetCasesFor_ValidateBasicRegionData_Success ), DynamicDataSourceType.Method ) ]
		public void Test_ValidateBasicRegionData_Success( Token[] input )
		{
			RegionDataValidator.ValidateBasicRegionData( input, TestData.RegionName, TestData.AllowedTokenNames );
		}

		[TestMethod ]
		[ ExpectedException( typeof( InvalidNameException ) ) ]
		[ DynamicData( nameof( GetCasesFor_ValidateBasicRegionData_ThrowsInvalidNameException ), DynamicDataSourceType.Method ) ]
		public void Test_ValidateBasicRegionData_ThrowsInvalidNameException( Token[] input )
		{
			RegionDataValidator.ValidateBasicRegionData( input, TestData.RegionName, TestData.AllowedTokenNames );
		}

		[TestMethod ]
		[ ExpectedException( typeof( MissingValueException ) ) ]
		[ DynamicData( nameof( GetCasesFor_ValidateBasicRegionData_ThrowsMissingValueException ), DynamicDataSourceType.Method ) ]
		public void Test_ValidateBasicRegionData_ThrowsMissingValueException( Token[] input )
		{
			RegionDataValidator.ValidateBasicRegionData( input, TestData.RegionName, TestData.AllowedTokenNames );
		}


		private static IEnumerable<object[]> GetCasesFor_RejectDuplicateTokenValues_Success()
		{
			yield return new TestData.DataContainer_RejectDuplicateTokenValues()
			{
				Input = TestData.TokenList_ContainsUniqueValues,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_RejectDuplicateTokenValues_ThrowsDuplicateValueException()
		{
			yield return new TestData.DataContainer_RejectDuplicateTokenValues()
			{
				Input = TestData.TokenList_ContainsDuplicateValues,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_ValidateBasicRegionData_Success()
		{
			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_ContainsUniqueValues,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_ContainsDuplicateValues,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_ValidateBasicRegionData_ThrowsInvalidNameException()
		{
			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_ContainsInvalidTokenName,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> GetCasesFor_ValidateBasicRegionData_ThrowsMissingValueException()
		{
			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_HasNoDefinitionTokens,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_HasNoRegionToken,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public const string AllowedTokenName0 = "valid-0";
			public const string AllowedTokenName1 = "valid-1";
			public static string[] AllowedTokenNames { get; } =
			{
				AllowedTokenName0,
				AllowedTokenName1,
			};
			public const string RegionName = "region-name";
			public static Token TokenWithInvalidName => new()
			{
				Name = "invalid",
				Value = "value",
			};
			public static Token TokenWithDuplicateValue0 => new()
			{
				Name = AllowedTokenName0,
				Value = "duplicate",
			};
			public static Token TokenWithDuplicateValue1 => new()
			{
				Name = AllowedTokenName1,
				Value = "duplicate",
			};
			public static Token TokenWithValidDataA0 => new()
			{
				Name = AllowedTokenName0,
				Value = "a0-value",
				Body = new()
				{
					TokenWithValidDataB0,
				},
			};
			public static Token TokenWithValidDataA1 => new()
			{
				Name = AllowedTokenName1,
				Value = "a1-value",
			};
			public static Token TokenWithValidDataB0 => new()
			{
				Name = AllowedTokenName0,
				Value = "b0-value",
			};
			public static Token[] TokenList_ContainsDuplicateValues { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						TokenWithDuplicateValue0,
						TokenWithDuplicateValue1,
					},
				},
				TokenWithDuplicateValue0,
				TokenWithDuplicateValue1,
			};
			public static Token[] TokenList_ContainsInvalidTokenName { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						TokenWithInvalidName,
					},
				},
				TokenWithInvalidName,
			};
			public static Token[] TokenList_ContainsUniqueValues { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						TokenWithValidDataA0,
						TokenWithValidDataA1,
					},
				},
				TokenWithValidDataA0,
				TokenWithValidDataA1,
				TokenWithValidDataB0,
			};
			public static Token[] TokenList_HasNoDefinitionTokens { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
				}
			};
			public static Token[] TokenList_HasNoRegionToken { get; } =
			{
				TokenWithValidDataA0,
			};


			public struct DataContainer_RejectDuplicateTokenValues
			{
				public Token[] Input { get; set; }


				public object[] ToObjectArray()
				{
					return new object[] { Input };
				}
			}

			public struct DataContainer_ValidateBasicRegionData
			{
				public Token[] Input { get; set; }


				public object[] ToObjectArray()
				{
					return new object[] { Input };
				}
			}
		}
	}
}