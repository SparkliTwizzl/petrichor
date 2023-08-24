using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.Logging;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[ TestClass ]
	public class TemplateParserTests
	{
		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		[ TestMethod ]
		[ DynamicData(
			nameof( Data_ParseTemplateData_Success ),
			DynamicDataSourceType.Method ) ]
		public void ParseTemplateData_Success( string[] expected, Token input )
		{
			var actual = TemplateParser.ParseTemplateData( input );
			Log.Separator();
			Log.Info( "expected = [[" );
			foreach ( var item in expected )
			{
				Log.Info( item );
			}
			Log.Info( "]]" );
			Log.Separator();
			Log.Info( "actual = [[" );
			foreach ( var item in actual )
			{
				Log.Info( item );
			}
			Log.Info( "]]" );
			Log.Separator();
			CollectionAssert.AreEqual( expected, actual );
		}

		[ TestMethod ]
		[ ExpectedException( typeof( InvalidDataException ) ) ]
		[ DynamicData(
			nameof( Data_ParseTemplateData_ThrowsInvalidDataException ),
			DynamicDataSourceType.Method ) ]
		public void ParseTemplateData_ThrowsInvalidDataException( Token input )
		{
			_ = TemplateParser.ParseTemplateData( input );
		}


		private static IEnumerable<object[]> Data_ParseTemplateData_Success()
		{
			yield return new TestData.DataContainer_ParseTemplateData_Success
			{
				Expected = TestData.ParsedTemplateList,
				Input = TestData.TemplateRegionToken_Standard,
			}.ToObjectArray();
		}

		private static IEnumerable<object[]> Data_ParseTemplateData_ThrowsInvalidDataException()
		{
			yield return new TestData.DataContainer_ParseTemplateData_ThrowsException
			{
				Input = TestData.TemplateRegionToken_ContainsInvalidManualTemplate_NoLeadingDivider,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTemplateData_ThrowsException
			{
				Input = TestData.TemplateRegionToken_ContainsInvalidManualTemplate_NoReplaceString,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTemplateData_ThrowsException
			{
				Input = TestData.TemplateRegionToken_ContainsInvalidManualTemplate_NoTrailingDivider,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTemplateData_ThrowsException
			{
				Input = TestData.TemplateRegionToken_ContainsInvalidTemplate_NoDivider,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTemplateData_ThrowsException
			{
				Input = TestData.TemplateRegionToken_ContainsInvalidTemplate_NoPrefixChar,
			}.ToObjectArray();

			yield return new TestData.DataContainer_ParseTemplateData_ThrowsException
			{
				Input = TestData.TemplateRegionToken_ContainsInvalidTemplate_NoReplaceField,
			}.ToObjectArray();
		}


		private static class TestData
		{
			public static string[] ParsedTemplateList { get; } = new string[]
			{
				"::!{replace-field}::replacement string {replacement-field}",
				"::{replace-field-0}-{replace-field-1}::replacement string {replacement-field}",
			};
			public static Token TemplateRegionToken_Standard => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template",
						Value = "!{replace-field}:replacement string {replacement-field}",
					},
					new()
					{
						Name = "template-manual",
						Value = "::{replace-field-0}-{replace-field-1}::replacement string {replacement-field}",
					},
				},
			};
			public static Token TemplateRegionToken_ContainsInvalidManualTemplate_NoLeadingDivider => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template-manual",
						Value = "{replace-field}::replacement string {replacement-field}",
					},
				},
			};
			public static Token TemplateRegionToken_ContainsInvalidManualTemplate_NoReplaceString => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template-manual",
						Value = "::::replacement string {replacement-field}",
					},
				},
			};
			public static Token TemplateRegionToken_ContainsInvalidManualTemplate_NoTrailingDivider => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template-manual",
						Value = "::{replace-field}replacement string {replacement-field}",
					},
				},
			};
			public static Token TemplateRegionToken_ContainsInvalidTemplate_NoDivider => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template",
						Value = "!{replace-field}replacement string {replacement-field}",
					},
				},
			};
			public static Token TemplateRegionToken_ContainsInvalidTemplate_NoPrefixChar => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template",
						Value = "{replace-field}:replacement string {replacement-field}",
					},
				},
			};
			public static Token TemplateRegionToken_ContainsInvalidTemplate_NoReplaceField => new()
			{
				Name = "region",
				Value = "templates",
				Body = new()
				{
					new()
					{
						Name = "template",
						Value = "!:replacement string {replacement-field}",
					},
				},
			};


			public struct DataContainer_ParseTemplateData_Success
			{
				public string[] Expected { get; set; }
				public Token Input { get; set; }

				public object[] ToObjectArray() => new object[]
				{
					Expected,
					Input,
				};
			}

			public struct DataContainer_ParseTemplateData_ThrowsException
			{
				public Token Input { get; set; }

				public object[] ToObjectArray() => new object[]
				{
					Input,
				};
			}
		}
	}
}