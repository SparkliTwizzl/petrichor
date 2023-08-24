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


		private static IEnumerable<object[]> Data_RejectDuplicateTokenValues_Success()
		{
			yield return new TestData.DataContainer_RejectDuplicateTokenValues()
			{
				Input = TestData.TokenList_HasUniqueValues,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_RejectDuplicateTokenValues_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_RejectDuplicateTokenValues_Success(
			Token[] input )
		{
			RegionDataValidator.RejectDuplicateTokenValues( input, TestData.RegionName );
		}


		private static IEnumerable<object[]> Data_RejectDuplicateTokenValues_ThrowsDuplicateValueException()
		{
			yield return new TestData.DataContainer_RejectDuplicateTokenValues()
			{
				Input = TestData.TokenList_HasDuplicateValues,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ ExpectedException( typeof( DuplicateValueException ) ) ]
		[ DynamicData(
			nameof( Data_RejectDuplicateTokenValues_ThrowsDuplicateValueException ),
			DynamicDataSourceType.Method ) ]
		public void Test_RejectDuplicateTokenValues_ThrowsDuplicateValueException(
			Token[] input )
		{
			RegionDataValidator.RejectDuplicateTokenValues( input, TestData.RegionName );
		}


		private static IEnumerable<object[]> Data_RejectNestedTokens_Success()
		{
			yield return new TestData.DataContainer_RejectNestedTokens()
			{
				Input = TestData.TokenList_HasNoNestedTokens,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ DynamicData(
			nameof( Data_RejectNestedTokens_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_RejectNestedTokens_Success(
			Token[] input )
		{
			RegionDataValidator.RejectNestedTokens( input, TestData.RegionName );
		}


		private static IEnumerable<object[]> Data_RejectNestedTokens_ThrowsInvalidDataException()
		{
			yield return new TestData.DataContainer_RejectNestedTokens()
			{
				Input = TestData.TokenList_HasNestedTokens,
			}.ToObjectArray();
		}

		[ TestMethod ]
		[ ExpectedException( typeof( InvalidDataException ) ) ]
		[ DynamicData(
			nameof( Data_RejectNestedTokens_ThrowsInvalidDataException ),
			DynamicDataSourceType.Method ) ]
		public void Test_RejectNestedTokens_ThrowsInvalidDataException(
			Token[] input )
		{
			RegionDataValidator.RejectNestedTokens( input, TestData.RegionName );
		}


		private static IEnumerable<object[]> Data_ValidateBasicRegionData_Success()
		{
			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_HasUniqueValues,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_HasDuplicateValues,
			}.ToObjectArray();
		}

		[TestMethod ]
		[ DynamicData(
			nameof( Data_ValidateBasicRegionData_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_ValidateBasicRegionData_Success(
			Token[] input )
		{
			RegionDataValidator.ValidateBasicRegionData( input, TestData.RegionName, TestData.AllowedTokenNames );
		}


		private static IEnumerable<object[]> Data_ValidateBasicRegionData_ThrowsInvalidNameException()
		{
			yield return new TestData.DataContainer_ValidateBasicRegionData()
			{
				Input = TestData.TokenList_HasInvalidTokenName,
			}.ToObjectArray();
		}

		[TestMethod ]
		[ ExpectedException( typeof( InvalidNameException ) ) ]
		[ DynamicData(
			nameof( Data_ValidateBasicRegionData_ThrowsInvalidNameException ),
			DynamicDataSourceType.Method ) ]
		public void Test_ValidateBasicRegionData_ThrowsInvalidNameException(
			Token[] input )
		{
			RegionDataValidator.ValidateBasicRegionData( input, TestData.RegionName, TestData.AllowedTokenNames );
		}


		private static IEnumerable<object[]> Data_ValidateBasicRegionData_ThrowsMissingValueException()
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

		[TestMethod ]
		[ ExpectedException( typeof( MissingValueException ) ) ]
		[ DynamicData(
			nameof( Data_ValidateBasicRegionData_ThrowsMissingValueException ),
			DynamicDataSourceType.Method ) ]
		public void Test_ValidateBasicRegionData_ThrowsMissingValueException(
			Token[] input )
		{
			RegionDataValidator.ValidateBasicRegionData( input, TestData.RegionName, TestData.AllowedTokenNames );
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
			public static Token Token_InvalidName => new()
			{
				Name = "invalid",
				Value = "value",
			};
			public static Token Token_DuplicateValue0 => new()
			{
				Name = AllowedTokenName0,
				Value = "duplicate",
			};
			public static Token Token_DuplicateValue1 => new()
			{
				Name = AllowedTokenName1,
				Value = "duplicate",
			};
			public static Token Token_ValidDataA0 => new()
			{
				Name = AllowedTokenName0,
				Value = "a0-value",
				Body = new()
				{
					Token_ValidDataB0,
				},
			};
			public static Token Token_ValidDataA1 => new()
			{
				Name = AllowedTokenName1,
				Value = "a1-value",
			};
			public static Token Token_ValidDataB0 => new()
			{
				Name = AllowedTokenName0,
				Value = "b0-value",
			};
			public static Token[] TokenList_HasDuplicateValues { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						Token_DuplicateValue0,
						Token_DuplicateValue1,
					},
				},
				Token_DuplicateValue0,
				Token_DuplicateValue1,
			};
			public static Token[] TokenList_HasInvalidTokenName { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						Token_InvalidName,
					},
				},
				Token_InvalidName,
			};
			public static Token[] TokenList_HasUniqueValues { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						Token_ValidDataA0,
						Token_ValidDataA1,
					},
				},
				Token_ValidDataA0,
				Token_ValidDataA1,
				Token_ValidDataB0,
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
				Token_ValidDataA0,
			};
			public static Token[] TokenList_HasNestedTokens { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						Token_ValidDataA0,
					},
				},
				Token_ValidDataA0,
			};
			public static Token[] TokenList_HasNoNestedTokens { get; } =
			{
				new()
				{
					Name = "region",
					Value = RegionName,
					Body = new()
					{
						Token_ValidDataA1,
					},
				},
				Token_ValidDataA1,
			};


			public struct DataContainer_RejectDuplicateTokenValues
			{
				public Token[] Input { get; set; }

				public object[] ToObjectArray()
				{
					return new object[] { Input };
				}
			}

			public struct DataContainer_RejectNestedTokens
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