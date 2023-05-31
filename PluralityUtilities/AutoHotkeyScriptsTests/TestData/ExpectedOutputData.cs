﻿using PluralityUtilities.AutoHotkeyScripts.Containers;


namespace PluralityUtilities.AutoHotkeyScripts.Tests.TestData
{
	public static class ExpectedOutputData
	{
		public static readonly string[] CreatedMacroData = new string[]
		{
			"::@sm:: #Sam",
			"::@sm/:: #Sam $(they/them) &-> a person",
			"::@smy:: #Sammy",
			"::@smy/:: #Sammy $(they/them) &-> a person",
			"::@ax:: #Alex",
			"::@ax/:: #Alex $() &",
		};

		public static readonly string[] GeneratedOutputData = new string[]
		{
			"#SingleInstance Force",
			"",
			"::@sm:: #Sam",
			"::@sm/:: #Sam $(they/them) &-> a person",
			"::@smy:: #Sammy",
			"::@smy/:: #Sammy $(they/them) &-> a person",
			"::@ax:: #Alex",
			"::@ax/:: #Alex $() &",
		};

		public static readonly Entry[] ParsedInputData = new Entry[]
		{
			new Entry()
			{
				Identities =
				{
					new Identity()
					{
						Name = "Sam",
						Tag = "sm",
					},
					new Identity()
					{
						Name = "Sammy",
						Tag = "smy",
					},
				},
				Pronoun = "they/them",
				Decoration = "-> a person",
			},
			new Entry()
			{
				Identities =
				{
					new Identity()
					{
						Name = "Alex",
						Tag = "ax",
					}
				},
			},
		};

		public static readonly string[] ParsedTemplateData = new string[]
		{
			"::@`tag`:: #`name`",
			"::@`tag`/:: #`name` $(`pronoun`) &`decoration`",
		};
	}
}
