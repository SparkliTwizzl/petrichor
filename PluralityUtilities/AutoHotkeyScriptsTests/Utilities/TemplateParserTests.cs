﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluralityUtilities.AutoHotkeyScripts.Tests.TestData;
using PluralityUtilities.AutoHotkeyScriptsTests.TestData;
using PluralityUtilities.TestCommon.Utilities;


namespace PluralityUtilities.AutoHotkeyScripts.Utilities.Tests
{
	[TestClass]
	public class TemplateParserTests
	{
		[TestInitialize]
		public void Setup()
		{
			TestUtilities.InitializeLoggingForTests();
		}


		[TestMethod]
		public void CreateAllMacrosFromTemplatesTest_Success()
		{
			var people = InputData.TemplateParserData.ValidPeople;
			var templates = InputData.TemplateParserData.ValidTemplates;
			var results = TemplateParser.CreateAllMacrosFromTemplates(people, templates);
			var expected = ExpectedOutputData.CreatedMacroData;
			var actual = results.ToArray();
			CollectionAssert.AreEqual(expected, actual);
		}

		[TestMethod]
		[DataRow("TemplateParser_ValidTemplates.txt")]
		public void ParseTemplatesFromFileTest_Success(string inputFile)
		{
			var filePath = TestUtilities.LocateInputFile(inputFile);

			Assert.Fail();
		}
	}
}