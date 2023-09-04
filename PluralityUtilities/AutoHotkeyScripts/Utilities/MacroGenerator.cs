using PluralityUtilities.Common.Containers;
using PluralityUtilities.Common.Utilities;

namespace PluralityUtilities.AutoHotkeyScripts.Utilities
{
	public class MacroGenerator
	{
		private Dictionary<string, string[]> Fields { get; set; } = new();
		private string[] Templates { get; set; } = Array.Empty<string>();
		private Token[] Entries { get; set; } = Array.Empty<Token>();


		public MacroGenerator() { }
		public MacroGenerator( Dictionary<string, string[]> fields, string[] templates, Token[] entries )
		{
			Fields = fields;
			Templates = templates;
			Entries = entries;
		}


		public string[] GenerateMacros()
		{
			var result = new List<string>();
			foreach ( var entry in Entries )
			{
				result.AddRange( GenerateMacrosFromEntry( entry ) );
			}
			return result.ToArray();
		}


		private Dictionary<string, string> CreateFindAndReplaceTableForToken( Token token )
		{
			var result = new Dictionary<string, string>();
			var flattenedToken = TokenDataParser.FlattenTokenTree( token );
			foreach ( var field in flattenedToken )
			{
				result.Add( field.Name, field.Value );
			}
			return result;
		}

		private Token[] SeparateInstancesOfSameFields( Token token )
		{
			// USE RECURSION TO BUILD UNIQUE TOKENS WITH DEPTH-FIRST TRAVERSAL
			// ie, at top level, create a new token, then at each level down, take a copy of the previous level's token and return a list of copies, adding every unique combination of fields
			var result = new List<Token>();
			//TODO prep batches of field data to be parsed into macros
			//TODO convert single field with duplicate fields into many copies with only one instance of each field, ie copy field and remove duplicates at every level
			return result.ToArray();
		}

		private List<string> GenerateMacrosFromEntry( Token entry )
		{
			var result = new List<string>();
			var uniqueEntries = SeparateInstancesOfSameFields( entry );
			result.AddRange( GenerateMacrosFromUniqueEntries( uniqueEntries ) );
			return result;
		}

		private List<string> GenerateMacrosFromUniqueEntries( Token[] uniqueEntries )
		{
			var result = new List<string>();
			//TODO loop to create batches of macros
			foreach ( var uniqueEntry in uniqueEntries )
			{
				var findAndReplaceTable = CreateFindAndReplaceTableForToken( uniqueEntry );
			}
			return result;
		}


		/*
		 * "::!{a}{b}::{c} text {d} {a}"
		 *     a = "{c}"
		 *     b = "{c}"
		 *     c = "{a}"
		 *     d = "{a}"
		 *     result should be "::!{c}{c}::{a} text {a} {c}"
		 */

		/*
		 * "::!{a}{b}::{c} text {d} {a}"
		 * "::!{c}{b}::{c} text {d} {c}" <- replace {a} with value
		 * "::!{c}{c}::{c} text {d} {c}" <- replace {b} with value
		 * "::!{a}{a}::{a} text {d} {a}" <- replace {c} with value
		 * "::!{a}{a}::{a} text {a} {a}" <- replace {d} with value
		 *
		 * "::!{a}{a}::{a} text {a} {a}" <- actual
		 * "::!{c}{c}::{a} text {a} {c}" <- expected
		 *
		 * doesnt work
		 */

		/*
		 * "::!{a}{b}::{c} text {d} {a}"
		 * "::!", "{a}", "{b}", "::", "{c}", " text ", "{d}", " ", "{a}" <- chunk string to replace in parts
		 * ["::!"], "{a}",  "{b}",  "::",  "{c}",  " text ",  "{d}",  " ",  "{a}"  <- iteratively replace
		 *  "::!", ["{c}"], "{b}",  "::",  "{c}",  " text ",  "{d}",  " ",  "{a}"  <- iteratively replace
		 *  "::!",  "{c}", ["{c}"], "::",  "{c}",  " text ",  "{d}",  " ",  "{a}"  <- iteratively replace
		 *  "::!",  "{c}",  "{c}", ["::"], "{c}",  " text ",  "{d}",  " ",  "{a}"  <- iteratively replace
		 *  "::!",  "{c}",  "{c}",  "::", ["{a}"], " text ",  "{d}",  " ",  "{a}"  <- iteratively replace
		 *  "::!",  "{c}",  "{c}",  "::",  "{a}", [" text "], "{d}",  " ",  "{a}"  <- iteratively replace
		 *  "::!",  "{c}",  "{c}",  "::",  "{a}",  " text ", ["{a}"], " ",  "{a}"  <- iteratively replace
		 *  "::!",  "{c}",  "{c}",  "::",  "{a}",  " text ",  "{a}", [" "], "{a}"  <- iteratively replace
		 *  "::!",  "{c}",  "{c}",  "::",  "{a}",  " text ",  "{a}",  " ", ["{c}"] <- iteratively replace
		 * "::!{c}{c}::{a} text {a} {c}" <- recombine
		 *
		 * "::!{c}{c}::{a} text {a} {c}" <- actual
		 * "::!{c}{c}::{a} text {a} {c}" <- expected
		 *
		 * should work
		 */
	}
}
