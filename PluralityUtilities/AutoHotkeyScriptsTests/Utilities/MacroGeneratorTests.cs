using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.Common.Containers;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[ TestClass ]
	public class MacroGeneratorTests
	{
		private MacroGenerator? MacroGenerator { get; set; }


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
			MacroGenerator = new MacroGenerator( fields: fields, templates: templates, entries: entries );
			var actual = MacroGenerator.GenerateMacros();
			CollectionAssert.AreEqual( expected, actual );
		}


		private static class TestData
		{
			public static Token[] Entries { get; } = new Token[]
			{
				new()
				{
					Name = "a0",
					Value = "a0_0",
					Body = new()
					{
						new()
						{
							Name = "b0",
							Value = "b0_0",
							Body = new()
							{
								new()
								{
									Name = "c0",
									Value = "c0_0",
								},
								new()
								{
									Name = "c0",
									Value = "c0_1",
								},
							}
						},
						new()
						{
							Name = "b0",
							Value = "b0_1",
							Body = new()
							{
								new()
								{
									Name = "c0",
									Value = "c0_2",
								},
								new()
								{
									Name = "c0",
									Value = "c0_3",
								},
							}
						},
						new()
						{
							Name = "b1",
							Value = "b1_0",
						},
					}
				},
				new()
				{
					Name = "a1",
					Value = "a1_0",
				},
			};
			public static string[] Expected { get; } = new string[]
			{
				"::!a0_0::b0_0 c0_0",
				"::{a1}-a1_0::{b1}-b1_0 {b0}-c0_0",
				"::!a0_0::b0_0 c0_1",
				"::{a1}-a1_0::{b1}-b1_0 {b0}-c0_1",
				"::!a0_0::b0_1 c0_2",
				"::{a1}-a1_0::{b1}-b1_0 {b0}-c0_2",
				"::!a0_0::b0_1 c0_3",
				"::{a1}-a1_0::{b1}-b1_0 {b0}-c0_3",
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
				@"::\{a1\}-{a1}::\{b1\}-{b1} \{c0\}-{c0}",
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