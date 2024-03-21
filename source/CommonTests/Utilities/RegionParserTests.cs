﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Petrichor.Common.Containers;
using Petrichor.Common.Exceptions;
using Petrichor.Common.Syntax;
using Petrichor.TestShared.Utilities;


namespace Petrichor.Common.Utilities.Tests
{
	[TestClass]
	public class RegionParserTests
	{
		public struct TestData
		{
			public static RegionParserDescriptor<IndexedString> ParserDescriptor => new()
			{
				MaxAllowedTokenInstances = new()
				{
					{ TokenName, 1 },
				},
				MinRequiredTokenInstances = new()
				{
					{ TokenName, 1 },
				},
				RegionName = RegionName,
				TokenHandlers = new()
				{
					{ TokenName, ( IndexedString[] regionData, int tokenStartIndex, IndexedString result ) => new() },
				},
			};
			public static IndexedString[] RegionData_MismatchedRegionClose => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				$"\t{ Token } { TokenValue }",
				Tokens.RegionClose,
			} );
			public static IndexedString[] RegionData_MismatchedRegionOpen => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				Tokens.RegionOpen,
				$"\t{ Token } { TokenValue }",
			} );
			public static IndexedString[] RegionData_TokenWithNoValue => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				Tokens.RegionOpen,
				$"\t{ Token }",
				Tokens.RegionClose,
			} );
			public static IndexedString[] RegionData_TooFewTokenInstances => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				Tokens.RegionOpen,
				Tokens.RegionClose,
			} );
			public static IndexedString[] RegionData_TooManyTokenInstances => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				Tokens.RegionOpen,
				$"\t{ Token } { TokenValue }",
				$"\t{ Token } { TokenValue }",
				Tokens.RegionClose,
			} );
			public static IndexedString[] RegionData_UnrecognizedToken => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				Tokens.RegionOpen,
				$"\tunknown-token{ OperatorChars.TokenValueDivider } value",
				Tokens.RegionClose,
			} );
			public static IndexedString[] RegionData_Valid => IndexedString.IndexStringArray( new[]
			{
				RegionToken,
				$"{ Tokens.RegionOpen } { Tokens.LineComment } inline comment",
				$"\t{ Token } { TokenValue }",
				Tokens.RegionClose,
			} );
			public static string RegionName => nameof( RegionParserTests );
			public static string RegionToken => $"{RegionName}{OperatorChars.TokenValueDivider}";
			public static string Token => $"{TokenName}{OperatorChars.TokenValueDivider}";
			public static string TokenName => "token-name";
			public static string TokenValue => "Value";
		}


		public RegionParser<IndexedString>? parser;


		[TestInitialize]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();

			parser = new( TestData.ParserDescriptor );
		}


		[TestMethod]
		[DynamicData( nameof( Parse_Test_Success_Data ), DynamicDataSourceType.Property )]
		public void Parse_Test_Success( IndexedString[] regionData )
		{
			var expectedResult = new IndexedString();
			var actualResult = parser!.Parse( regionData );
			Assert.AreEqual( expectedResult, actualResult );

			var expectedLinesParsed = regionData.Length;
			var actualLinesParsed = parser!.LinesParsed;
			Assert.AreEqual( expectedLinesParsed, actualLinesParsed );
		}

		public static IEnumerable<object[]> Parse_Test_Success_Data
		{
			get
			{
				yield return new object[] { TestData.RegionData_Valid };
				yield return new object[] { TestData.RegionData_TokenWithNoValue };
			}
		}

		[TestMethod]
		[ExpectedException( typeof( BracketException ) )]
		[DynamicData( nameof( Parse_Test_Throws_BracketException_Data ), DynamicDataSourceType.Property )]
		public void Parse_Test_Throws_BracketException( IndexedString[] regionData ) => _ = parser!.Parse( regionData );

		public static IEnumerable<object[]> Parse_Test_Throws_BracketException_Data
		{
			get
			{
				yield return new object[] { TestData.RegionData_MismatchedRegionClose };
				yield return new object[] { TestData.RegionData_MismatchedRegionOpen };
			}
		}

		[TestMethod]
		[ExpectedException( typeof( TokenCountException ) )]
		[DynamicData( nameof( Parse_Test_Throws_TokenCountException_Data ), DynamicDataSourceType.Property )]
		public void Parse_Test_Throws_TokenCountException( IndexedString[] regionData ) => _ = parser!.Parse( regionData );

		public static IEnumerable<object[]> Parse_Test_Throws_TokenCountException_Data
		{
			get
			{
				yield return new object[] { TestData.RegionData_TooFewTokenInstances };
				yield return new object[] { TestData.RegionData_TooManyTokenInstances };
			}
		}

		[TestMethod]
		[ExpectedException( typeof( TokenNameException ) )]
		[DynamicData( nameof( Parse_Test_Throws_TokenNameException_Data ), DynamicDataSourceType.Property )]
		public void Parse_Test_Throws_TokenNameException( IndexedString[] regionData ) => _ = parser!.Parse( regionData );

		public static IEnumerable<object[]> Parse_Test_Throws_TokenNameException_Data
		{
			get
			{
				yield return new object[] { TestData.RegionData_UnrecognizedToken };
			}
		}
	}
}