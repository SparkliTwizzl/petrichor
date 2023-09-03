using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[ TestClass ]
	public class MacroGeneratorTests
	{
		private MacroGenerator? macroGenerator { get; set; }


		[ TestInitialize ]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		private static IEnumerable<object[]> Data_GenerateMacros_Success()
		{
			yield return new TestData.DataContainer_GenerateMacros_Success
			{
				Expected = TestData.Expected,
				Fields = TestData.Fields,
				Templates = TestData.Templates,
				Entries = TestData.Entries,
			}.ToObjectArray();
		}

		[TestMethod ]
		[ DynamicData(
			nameof( Data_GenerateMacros_Success ),
			DynamicDataSourceType.Method ) ]
		public void Test_GenerateMacros_Success(
			string[] expected, Dictionary<string, string[]> fields, string[] templates, Token[] entries )
		{
			macroGenerator = new MacroGenerator( fields: fields, templates: templates, entries: entries );
			var actual = macroGenerator.GenerateMacros();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static class TestData
		{
			public static Token[] Entries { get; } = new Token[]
			{
				new()
				{
					Name = "a0",
					Value = "a0-value",
					Body = new()
					{
						new()
						{
							Name = "b0",
							Value = "b0-value-0",
							Body = new()
							{
								new()
								{
									Name = "c0",
									Value = "c0-value-0",
								},
								new()
								{
									Name = "c0",
									Value = "c0-value-1",
								},
							}
						},
						new()
						{
							Name = "b0",
							Value = "b0-value-1",
							Body = new()
							{
								new()
								{
									Name = "c0",
									Value = "c0-value-2",
								},
								new()
								{
									Name = "c0",
									Value = "c0-value-3",
								},
							}
						},
						new()
						{
							Name = "b1",
							Value = "b1-value",
						},
					}
				},
				new()
				{
					Name = "a1",
					Value = "a1-value",
				},
			};
			public static string[] Expected { get; } = new string[]
			{
				"::!a0-value::b0-value-0 c0-value-0",
				"::-a1-value-::b1-value c0-value-0",
				"::!a0-value::b0-value-0 c0-value-1",
				"::-a1-value-::b1-value c0-value-1",
				"::!a0-value::b0-value-1 c0-value-2",
				"::-a1-value-::b1-value c0-value-2",
				"::!a0-value::b0-value-1 c0-value-3",
				"::-a1-value-::b1-value c0-value-3",
			};
			public static Dictionary<string, string[]> Fields { get; } = new()
			{
				{ "fields", new string[] { "a0", "a1" } },
				{ "a0", new string[] { "b0", "b1" } },
				{ "a1", Array.Empty<string>() },
				{ "b0", new string[] { "c0" } },
				{ "b1", Array.Empty<string>() },
				{ "c0", Array.Empty<string>() },
			};
			public static string[] Templates { get; } = new string[]
			{
				"::!{a0}::{b0} {c0}",
				"::-{a1}-::{b1} {c0}",
			};


			public struct DataContainer_GenerateMacros_Success
			{
				public string[] Expected { get; set; }
				public Token[] Entries { get; set; }
				public Dictionary<string, string[]> Fields { get; set; }
				public string[] Templates { get; set; }

				public object[] ToObjectArray() => new object[]
				{
						Expected,
						Fields,
						Templates,
						Entries,
				};
			}
		}
	}
}